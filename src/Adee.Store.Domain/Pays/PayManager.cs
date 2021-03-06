using Adee.Store.CallbackRequests;
using Adee.Store.Domain.Pays;
using Adee.Store.Domain.Tenants;
using Adee.Store.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.SettingManagement;

namespace Adee.Store.Pays
{
    public class PayManager : DomainService, ITransientDependency
    {
        private readonly IPayOrderRepository _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly IPayParameterRepository _payParameterRepository;
        private readonly IRepository<PayNotify> _payNotifyRepository;
        private readonly IRepository<PayRefund> _payRefundRepository;
        private readonly IRepository<CallbackRequest> _callbackRequestRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ISettingManager _settingManager;
        private readonly IDistributedCache<AssertNotifyResponse> _assertNotifyCache;
        private readonly IOptions<PayOptions> _payOptions;
        private readonly IOptions<AppOptions> _appOptions;
        private readonly IAbpLazyServiceProvider _serviceProvider;
        private readonly IObjectMapper _objectMapper;
        private readonly ICurrentTenantExt _currentTenantExt;
        private readonly QueryOrderCacheItemManager _queryOrderCacheItemManager;

        public PayManager(
            IPayOrderRepository payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            IPayParameterRepository payParameterRepository,
            IRepository<PayNotify> payNotifyRepository,
            IRepository<PayRefund> payRefundRepository,
            IRepository<CallbackRequest> callbackRequestRepository,
            IBackgroundJobManager backgroundJobManager,
            ISettingManager settingManager,
            IDistributedCache<AssertNotifyResponse> assertNotifyCache,
            IOptions<PayOptions> payOptions,
            IOptions<AppOptions> appOptions,
            IAbpLazyServiceProvider serviceProvider,
            IObjectMapper objectMapper,
            ICurrentTenantExt currentTenantExt,
            QueryOrderCacheItemManager queryOrderCacheItemManager
            )
        {
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _payParameterRepository = payParameterRepository;
            _payNotifyRepository = payNotifyRepository;
            _payRefundRepository = payRefundRepository;
            _callbackRequestRepository = callbackRequestRepository;
            _backgroundJobManager = backgroundJobManager;
            _settingManager = settingManager;
            _assertNotifyCache = assertNotifyCache;
            _payOptions = payOptions;
            _appOptions = appOptions;
            _serviceProvider = serviceProvider;
            _objectMapper = objectMapper;
            _currentTenantExt = currentTenantExt;
            _queryOrderCacheItemManager = queryOrderCacheItemManager;
        }

        // public async Task<object> Excution(PayTaskType type, string payTaskContent)
        // {
        //     var payOrganizationType = payTaskContent.AsAnonymousObject(new { PayOrganizationType = PayOrganizationType.Unknown });
        //     CheckHelper.IsTrue(payOrganizationType.PayOrganizationType!= PayOrganizationType.Unknown, "????????????????????????");

        //     var payProvider = GetPayProvider(payOrganizationType.PayOrganizationType);
        //     payProvider.Excute()
        // }

        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public async Task<RefundResult> Refund(string orderId, int money)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.PayOrderId == orderId || p.BusinessOrderId == orderId || p.PayOrganizationOrderId == orderId);
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            var payParameter = await GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var payRefund = await _payRefundRepository.InsertAsync(new PayRefund(GuidGenerator.Create())
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                RefundOrderId = $"Refund-{payOrder.PayOrderId}-{new Random(GetHashCode()).Next(100, 999)}",
                Money = money,
                Status = PayTaskStatus.Executing,
            }, true);

            var request = new RefundRequest
            {
                PayParameterValue = payParameter.Value,
                Money = money,
                PayOrderId = payOrder.PayOrderId,
                RefundOrderId = payRefund.RefundOrderId,
            };

            payOrder.RefundStatus = PayTaskStatus.Executing;
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Refund,
                Status = PayTaskStatus.Executing,
                OriginRequest = request.ToJsonString(),
            }, true);

            SuccessResponse response;
            try
            {
                response = await payProvider.Refund(request);
            }
            catch (Exception ex)
            {
                response = new SuccessResponse
                {
                    Status = PayTaskStatus.Faild,
                    ResponseMessage = ex.Message,
                    OriginRequest = request.ToJsonString(),
                    OriginResponse = ex.ToJsonString(),
                };
            }

            var result = new RefundResult
            {
                Status = response.Status,
                Message = response.ResponseMessage,
                PayOrderId = payOrder.PayOrderId,
                RefundOrderId = payRefund.RefundOrderId,
                BusinessOrderId = payOrder.BusinessOrderId,
            };

            payOrder.RefundStatus = response.Status;
            payOrder.RefundStatusMessage = response.ResponseMessage;
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            payRefund.Status = response.Status;
            payRefund.StatusMessage = response.ResponseMessage;
            payRefund = await _payRefundRepository.UpdateAsync(payRefund);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Refund,
                Status = response.Status,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                OriginResponse = response.OriginResponse,
                EncryptResponse = response.EncryptResponse,
            }, true);

            if (response.Status == PayTaskStatus.Faild || response.Status == PayTaskStatus.Success)
            {
                var notifyArgs = new PayNotifyArgs
                {
                    TenantId = payOrder.TenantId,
                    PayOrderId = payOrder.PayOrderId,
                    NotifyContent = result.ToJsonString(),
                    IsRefundNotify = true,
                };
                await _backgroundJobManager.EnqueueAsync(notifyArgs);

                return result;
            }

            if (response.Status == PayTaskStatus.Executing || response.Status == PayTaskStatus.Waiting || response.Status == PayTaskStatus.Normal)
            {
                var refundQueryArgs = new RefundQueryStatusArgs
                {
                    TenantId = payOrder.TenantId,
                    PayOrderId = payOrder.PayOrderId,
                    RefundPayOrderId = payRefund.RefundOrderId,
                    Money = money,
                };
                await _backgroundJobManager.EnqueueAsync(refundQueryArgs, delay: TimeSpan.FromSeconds(1));
            }

            return result;
        }

        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="payOrderId"></param>
        /// <returns></returns>
        public async Task<PayTaskResult> Cancel(string payOrderId)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.PayOrderId == payOrderId);
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            var payParameter = await GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            payOrder.CancelStatus = PayTaskStatus.Executing;
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            var request = new PayTaskRequest
            {
                PayTaskType = PayTaskType.Cancel,
                PayParameterValue = payParameter.Value,
                PayOrderId = payOrderId,
            };

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Cancel,
                Status = PayTaskStatus.Executing,
                OriginRequest = request.ToJsonString(),
            }, true);

            PayResponse response;
            try
            {
                response = await payProvider.Cancel(request);
            }
            catch (Exception ex)
            {
                response = new SuccessResponse
                {
                    Status = PayTaskStatus.Faild,
                    ResponseMessage = ex.Message,
                    OriginRequest = request.ToJsonString(),
                    OriginResponse = ex.ToJsonString(),
                };
            }

            payOrder.CancelStatus = response.Status;
            payOrder.CancelStatusMessage = response.ResponseMessage;
            payOrder.Status = response.Status;
            await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Cancel,
                Status = response.Status,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                OriginResponse = response.OriginResponse,
                EncryptResponse = response.EncryptResponse,
            }, true);

            return new PayTaskResult
            {
                Status = response.Status,
                Message = response.ResponseMessage,
            };
        }

        /// <summary>
        /// B2C??????
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OrderResult> B2C(B2C model)
        {
            var existBusinessOrderId = await _payOrderRepository.ExistBusinessOrderId(model.BusinessOrderId);
            CheckHelper.IsFalse(existBusinessOrderId, $"??????????????????{model.BusinessOrderId}?????????");

            var paymentType = GetPaymentTypeFromAuthCode(model.AuthCode);
            Check.NotNull(paymentType, nameof(paymentType));

            var payParameter = await GetPayParameter(paymentType);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var payOrder = new PayOrder(GuidGenerator.Create())
            {
                TenantId = CurrentTenant.Id,
                Money = model.Money,
                NotifyUrl = model.NotifyUrl,
                Title = model.Title,
                BusinessOrderId = model.BusinessOrderId,
                PayOrderId = new PayOrderId
                {
                    PaymentType = paymentType,
                    PayOrganizationType = payParameter.PayOrganizationType,
                    PaymethodType = PaymethodType.B2C,
                    SoftwareCode = _currentTenantExt.SoftwareCode,
                    BusinessType = model.BusinessType,
                }.ToString(),
                ParameterVersion = payParameter.Version,
                PaymentType = paymentType,
                PayOrganizationType = payParameter.PayOrganizationType,
                PaymethodType = PaymethodType.B2C,
                BusinessType = model.BusinessType,
                Status = PayTaskStatus.Executing,
                PayRemark = model.PayRemark,
            };
            payOrder = await _payOrderRepository.InsertAsync(payOrder);

            var request = new B2CRequest
            {
                AuthCode = model.AuthCode,
                NotifyUrl = $"{_appOptions.Value.SelfUrl}/api/store/notify",
                PayParameterValue = payParameter.Value,
                Money = model.Money,
                Title = model.Title,
                IPAddress = model.IPAddress,
                PayOrderId = payOrder.PayOrderId,
            };

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Pay,
                Status = PayTaskStatus.Executing,
                OriginRequest = request.ToJsonString(),
            }, true);

            SuccessResponse response;
            try
            {
                response = await payProvider.B2C(request);
            }
            catch (Exception ex)
            {
                response = new SuccessResponse
                {
                    Status = PayTaskStatus.Faild,
                    ResponseMessage = ex.Message,
                    OriginRequest = request.ToJsonString(),
                    OriginResponse = ex.ToJsonString(),
                };
            }

            var result = new OrderResult
            {
                Status = response.Status,
                Message = response.ResponseMessage,
                PayOrderId = request.PayOrderId,
                BusinessOrderId = payOrder.BusinessOrderId,
            };

            payOrder.Status = response.Status;
            payOrder.StatusMessage = response.ResponseMessage;
            if (response.Status == PayTaskStatus.Success)
            {
                payOrder.PayTime = response.PayTime;
                payOrder.PayOrganizationOrderId = response.PayOrganizationOrderId;
            }
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Pay,
                Status = payOrder.Status,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                OriginResponse = response.OriginResponse,
                EncryptResponse = response.EncryptResponse,
            }, true);

            if (response.Status == PayTaskStatus.Normal || response.Status == PayTaskStatus.Executing || response.Status == PayTaskStatus.Waiting)
            {
                await _backgroundJobManager.EnqueueAsync(new PayQueryStatusArgs
                {
                    TenantId = CurrentTenant.Id.Value,
                    PayOrderId = payOrder.PayOrderId,
                }, delay: TimeSpan.FromSeconds(1));
            }

            if (response.Status == PayTaskStatus.Success || response.Status == PayTaskStatus.Faild)
            {
                result.PayTime = response.PayTime;
                result.Money = response.Money;
                result.BusinessType = model.BusinessType;
                result.PaymentType = paymentType;
                result.PayOrganizationOrderId = response.PayOrganizationOrderId;

                await _queryOrderCacheItemManager.RemoveAsync(_objectMapper.Map<PayOrder, QueryOrderCacheItem>(payOrder));

                var notifyArgs = new PayNotifyArgs
                {
                    TenantId = payOrder.TenantId,
                    PayOrderId = payOrder.PayOrderId,
                    NotifyContent = result.ToJsonString(),
                };
                await _backgroundJobManager.EnqueueAsync(notifyArgs);
            }

            return result;
        }

        /// <summary>
        /// B2C??????
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PayUrlResult> C2B(C2B model)
        {
            var existBusinessOrderId = await _payOrderRepository.ExistBusinessOrderId(model.BusinessOrderId);
            CheckHelper.IsFalse(existBusinessOrderId, $"??????????????????{model.BusinessOrderId}?????????");

            CheckHelper.AreNotEqual(model.PaymentType, PaymentType.Unknown, message: "?????????????????????");
            CheckHelper.AreNotEqual(model.PaymentType, PaymentType.CCB, message: $"???????????????{PaymentType.CCB.GetDescription()}");

            var payParameter = await GetPayParameter(model.PaymentType);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var payOrder = new PayOrder(GuidGenerator.Create())
            {
                TenantId = CurrentTenant.Id,
                Money = model.Money,
                NotifyUrl = model.NotifyUrl,
                Title = model.Title,
                BusinessOrderId = model.BusinessOrderId,
                PayOrderId = new PayOrderId
                {
                    PaymentType = model.PaymentType,
                    PayOrganizationType = payParameter.PayOrganizationType,
                    PaymethodType = PaymethodType.C2B,
                    SoftwareCode = _currentTenantExt.SoftwareCode,
                    BusinessType = model.BusinessType,
                }.ToString(),
                ParameterVersion = payParameter.Version,
                PaymentType = model.PaymentType,
                PayOrganizationType = payParameter.PayOrganizationType,
                PaymethodType = PaymethodType.C2B,
                BusinessType = model.BusinessType,
                Status = PayTaskStatus.Waiting,
                PayRemark = model.PayRemark,
            };
            payOrder = await _payOrderRepository.InsertAsync(payOrder);

            var request = new C2BRequest
            {
                NotifyUrl = $"{_appOptions.Value.SelfUrl}/api/store/notify",
                PayParameterValue = payParameter.Value,
                Money = model.Money,
                Title = model.Title,
                IPAddress = model.IPAddress,
                PayOrderId = payOrder.PayOrderId,
                PaymentType = model.PaymentType,
                PayExpire = model.PayExpire,
            };

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Pay,
                Status = PayTaskStatus.Waiting,
                OriginRequest = request.ToJsonString(),
            }, true);

            PayUrlResponse response;
            try
            {
                response = await payProvider.C2B(request);
            }
            catch (Exception ex)
            {
                response = new PayUrlResponse
                {
                    Status = PayTaskStatus.Faild,
                    ResponseMessage = ex.Message,
                    OriginRequest = request.ToJsonString(),
                    OriginResponse = ex.ToJsonString(),
                };
            }

            var result = new PayUrlResult
            {
                Status = response.Status,
                Message = response.ResponseMessage,
                PayOrganizationOrderId = response.PayOrganizationOrderId,
                PayOrderId = payOrder.PayOrderId,
                BusinessOrderId = payOrder.BusinessOrderId,
                PayUrl = response.PayUrl,
            };

            payOrder.Status = response.Status;
            payOrder.StatusMessage = response.ResponseMessage;

            if (response.Status == PayTaskStatus.Success)
            {
                payOrder.Status = PayTaskStatus.Executing;
                payOrder.StatusMessage = "????????????????????????";
                payOrder.PayOrganizationOrderId = response.PayOrganizationOrderId;
            }
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Pay,
                Status = payOrder.Status,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                OriginResponse = response.OriginResponse,
                EncryptResponse = response.EncryptResponse,
            }, true);

            if (response.Status == PayTaskStatus.Success)
            {
                await _backgroundJobManager.EnqueueAsync(new PayQueryStatusArgs
                {
                    TenantId = CurrentTenant.Id.Value,
                    PayOrderId = payOrder.PayOrderId,
                }, delay: TimeSpan.FromSeconds(1));
            }

            return result;
        }

        /// <summary>
        /// B2C??????
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ParameterResult> JSApi(JSApi model)
        {
            var existBusinessOrderId = await _payOrderRepository.ExistBusinessOrderId(model.BusinessOrderId);
            CheckHelper.IsFalse(existBusinessOrderId, $"??????????????????{model.BusinessOrderId}?????????");

            CheckHelper.AreNotEqual(model.PaymentType, PaymentType.Unknown, message: "?????????????????????");
            CheckHelper.AreNotEqual(model.PaymentType, PaymentType.CCB, message: $"???????????????{PaymentType.CCB.GetDescription()}");

            var payParameter = await GetPayParameter(model.PaymentType);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var paymethodType = GetPaymethodType(model.PaymentType, model.TradeType);

            var payOrder = new PayOrder(GuidGenerator.Create())
            {
                TenantId = CurrentTenant.Id,
                Money = model.Money,
                NotifyUrl = model.NotifyUrl,
                Title = model.Title,
                BusinessOrderId = model.BusinessOrderId,
                PayOrderId = new PayOrderId
                {
                    PaymentType = model.PaymentType,
                    PayOrganizationType = payParameter.PayOrganizationType,
                    PaymethodType = paymethodType,
                    SoftwareCode = _currentTenantExt.SoftwareCode,
                    BusinessType = model.BusinessType,
                }.ToString(),
                ParameterVersion = payParameter.Version,
                PaymentType = model.PaymentType,
                PayOrganizationType = payParameter.PayOrganizationType,
                PaymethodType = paymethodType,
                BusinessType = model.BusinessType,
                Status = PayTaskStatus.Waiting,
                PayRemark = model.PayRemark,
            };
            payOrder = await _payOrderRepository.InsertAsync(payOrder);

            var request = new JSApiRequest
            {
                NotifyUrl = $"{_appOptions.Value.SelfUrl}/api/store/notify",
                PayParameterValue = payParameter.Value,
                Money = model.Money,
                Title = model.Title,
                IPAddress = model.IPAddress,
                PayOrderId = payOrder.PayOrderId,
                TradeType = model.TradeType,
                PaymentType = model.PaymentType,
                PayExpire = model.PayExpire,
                SubAppId = model.SubAppId,
                UserId = model.UserId,
            };

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Pay,
                Status = PayTaskStatus.Waiting,
                OriginRequest = request.ToJsonString(),
            }, true);

            JsApiResponse response;
            try
            {
                response = await payProvider.JSApi(request);
            }
            catch (Exception ex)
            {
                response = new JsApiResponse
                {
                    Status = PayTaskStatus.Faild,
                    ResponseMessage = ex.Message,
                    OriginRequest = request.ToJsonString(),
                    OriginResponse = ex.ToJsonString(),
                };
            }

            var result = new ParameterResult
            {
                Status = response.Status,
                Message = response.ResponseMessage,
                PayOrganizationOrderId = response.PayOrganizationOrderId,
                PayOrderId = payOrder.PayOrderId,
                Parameter = response.Parameter,
            };

            payOrder.Status = response.Status;
            payOrder.StatusMessage = response.ResponseMessage;

            if (response.Status == PayTaskStatus.Success)
            {
                payOrder.Status = PayTaskStatus.Executing;
                payOrder.StatusMessage = "????????????????????????";
                payOrder.PayOrganizationOrderId = response.PayOrganizationOrderId;
            }
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Pay,
                Status = payOrder.Status,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                OriginResponse = response.OriginResponse,
                EncryptResponse = response.EncryptResponse,
            }, true);

            if (response.Status == PayTaskStatus.Success)
            {
                await _backgroundJobManager.EnqueueAsync(new PayQueryStatusArgs
                {
                    TenantId = CurrentTenant.Id.Value,
                    PayOrderId = payOrder.PayOrderId,
                }, delay: TimeSpan.FromSeconds(1));
            }

            return result;
        }

        /// <summary>
        /// ??????????????????(??????)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderResult> GetQueryFromCache(string orderId)
        {
            var queryOrderCacheItem = await _queryOrderCacheItemManager.GetAsync(orderId);
            if (queryOrderCacheItem.IsNotNull())
            {
                return _objectMapper.Map<QueryOrderCacheItem, OrderResult>(queryOrderCacheItem);
            }

            var payOrder = await _payOrderRepository.FindAsync(p => p.BusinessOrderId == orderId || p.PayOrderId == orderId || p.PayOrganizationOrderId == orderId);
            CheckHelper.IsNotNull(payOrder, $"?????????{orderId} ?????????");

            queryOrderCacheItem = _objectMapper.Map<PayOrder, QueryOrderCacheItem>(payOrder);
            await _queryOrderCacheItemManager.SetAsync(queryOrderCacheItem);

            return _objectMapper.Map<QueryOrderCacheItem, OrderResult>(queryOrderCacheItem);
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="paymentType"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<PayParameter> GetPayParameter(PaymentType paymentType, long? version = null)
        {
            if (version.HasValue == false)
            {
                version = _currentTenantExt.PaypameterVersion;
            }
            CheckHelper.IsTrue(version.HasValue && version > 0, $"?????????????????????{CurrentTenant.Id}????????????????????????");

            var payParameterValue = await _settingManager.GetOrNullForCurrentTenantAsync(StoreSettings.PayParameterVersionKey, new { Version = version.Value, PaymentType = paymentType });
            if (payParameterValue.IsNotNull()) return payParameterValue.AsObject<PayParameter>();

            var payParameter = await _payParameterRepository.GetPayParameter(paymentType, version);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            await _settingManager.SetForCurrentTenantAsync(StoreSettings.PayParameterVersionKey, payParameter.ToJsonString(), new { Version = payParameter.Version, PaymentType = paymentType });

            return payParameter;
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="request"></param>
        /// <param name="payOrderId"></param>
        /// <returns></returns>
        public async Task AssertNotify(AssertNotifyRequest request)
        {
            var notify = new PayNotify
            {
                TenantId = CurrentTenant.Id,
                Status = PayTaskStatus.Waiting,
            };
            try
            {
                var callbackRequest = _objectMapper.Map<AssertNotifyRequest, CallbackRequest>(request);
                callbackRequest = await _callbackRequestRepository.InsertAsync(callbackRequest);

                notify.CallbackRequestId = callbackRequest.Id;
                notify = await _payNotifyRepository.InsertAsync(notify, autoSave: true);

                var payOrganizationTypes = Enum.GetValues<PayOrganizationType>();

                var assertNotifyResults = new Dictionary<PayOrganizationType, AssertNotifyResponse>();
                foreach (var payOrganizationType in payOrganizationTypes)
                {
                    try
                    {
                        var payProvider = GetPayProvider(payOrganizationType);
                        var result = await payProvider.AssertNotify(request);

                        if (result.Status != PayTaskStatus.Success) continue;

                        assertNotifyResults.Add(payOrganizationType, result);
                    }
                    catch (NotImplementedException ex)
                    {
                        Logger.LogDebug(ex, $"?????????{payOrganizationType}?????????AssertNotify??????");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"?????????{payOrganizationType}????????????????????????????????????{ex.Message}");
                    }
                }

                CheckHelper.IsNotNull(assertNotifyResults, $"????????????????????????????????????????????????????????????");
                if (assertNotifyResults.Count() != 1)
                {
                    Logger.LogCritical($"????????????????????????????????????????????????????????????????????????{assertNotifyResults.Select(p => p.Key).JoinAsString()}??????????????????{request.ToJsonString()}");
                    throw new UserFriendlyException($"????????????????????????????????????????????????????????????????????????{assertNotifyResults.Select(p => p.Key).JoinAsString()}");
                }

                var hitAssertNotifyResult = assertNotifyResults.Select(p => p.Value).Single();
                await _assertNotifyCache.SetAsync(hitAssertNotifyResult.PayOrderId, hitAssertNotifyResult);

                notify.Status = PayTaskStatus.Success;
                notify.PayOrderId = hitAssertNotifyResult.PayOrderId;
                notify.BusinessOrderId = hitAssertNotifyResult.BusinessOrderId;
                notify.PayOrganizationOrderId = hitAssertNotifyResult.PayOrganizationOrderId;
            }
            catch (Exception ex)
            {
                notify.Status = PayTaskStatus.Faild;
                notify.StatusMessage = ex.Message;

                throw new Exception(ex.Message, ex);
            }
            finally
            {
                await _payNotifyRepository.UpdateAsync(notify, autoSave: true);
            }
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="payOrganizationType"></param>
        /// <returns></returns>
        public IDefaultPayProvider GetPayProvider(PayOrganizationType payOrganizationType)
        {
            var payProviderTypes = _payOptions.Value.PayProviderTypes
                 .Where(p => p.CustomAttributes.Any(c => c.AttributeType == typeof(PayProviderAttribute)))
                 .Where(p => p.CustomAttributes.Any(c => c.ConstructorArguments.Any(a => a.ArgumentType == typeof(PayOrganizationType) && a.Value != null && (PayOrganizationType)a.Value == payOrganizationType)))
                 .ToList();

            CheckHelper.IsNotNull(payProviderTypes, $"???????????????{payOrganizationType}?????????");
            CheckHelper.AreEqual(payProviderTypes.Count(), 1, message: $"?????????????????????{payProviderTypes.Select(c => c.Name).JoinAsString()}?????????{payOrganizationType}");

            return (IDefaultPayProvider)_serviceProvider.LazyGetRequiredService(payProviderTypes.Single());
        }

        /// <summary>
        /// ??????????????????????????????
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        private PaymentType GetPaymentTypeFromAuthCode(string authCode)
        {
            CheckHelper.IsTrue(int.TryParse(authCode.Substring(0, 2), out var startCode), $"???????????????{authCode}");

            if (startCode >= 10 && startCode <= 15) // ??????????????????
            {
                return PaymentType.WechatPay;
            }
            if (startCode >= 20 && startCode <= 33) // ?????????????????????
            {
                return PaymentType.Alipay;
            }
            // if (System.Text.RegularExpressions.Regex.IsMatch(authCode, @"62\d0105\d{12}"))
            // {
            //     return PaymentType.CCB;
            // }
            if (startCode == 62 && authCode.Length == 19)
            {
                return PaymentType.UnionPay;
            }

            throw new UserFriendlyException($"???????????????{authCode}");
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="paymentType"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        private PaymethodType GetPaymethodType(PaymentType paymentType, JSApiTradeType tradeType)
        {
            if (paymentType == PaymentType.UnionPay && tradeType == JSApiTradeType.MP) return PaymethodType.UnionJSPay;

            if (paymentType == PaymentType.Alipay) return tradeType == JSApiTradeType.MINIPRO ? PaymethodType.AlipayMiniPro : PaymethodType.AlipayMP;

            if (paymentType == PaymentType.WechatPay) return tradeType == JSApiTradeType.MINIPRO ? PaymethodType.WechatMiniPro : PaymethodType.WechatMP;

            throw new UserFriendlyException($"???????????????????????????paymentType???{paymentType}???tradeType???{tradeType}");
        }
    }
}
