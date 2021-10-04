using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adee.Store.Domain.Pays;
using Adee.Store.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.SettingManagement;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Adee.Store.Domain.Tenants;
using Adee.Store.Pays.Utils.Helpers;

namespace Adee.Store.Pays
{
    public class PayManager : DomainService, ITransientDependency
    {
        /// <summary>
        /// 查询时长，秒
        /// </summary>
        public const int QueryDuration = 600;

        /// <summary>
        /// 退款时长，天
        /// </summary>
        public const int RefundDuration = 14;

        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly IPayParameterRepository _payParameterRepository;
        private readonly IRepository<PayNotify> _payNotifyRepository;
        private readonly IRepository<PayRefund> _payRefundRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ISettingManager _settingManager;
        private readonly ILogger<PayManager> _logger;
        private readonly IDistributedCache<QueryOrderCacheItem> _queryOrderCache;
        private readonly IDistributedCache<AssertNotifyResponse> _assertNotifyCache;
        private readonly IOptions<PayOptions> _payOptions;
        private readonly IAbpLazyServiceProvider _serviceProvider;
        private readonly IObjectMapper _objectMapper;
        private readonly ICurrentTenantExt _currentTenantExt;
        private readonly ICommonClient _commonClient;

        public PayManager(
            IRepository<PayOrder> payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            IPayParameterRepository payParameterRepository,
            IRepository<PayNotify> payNotifyRepository,
            IRepository<PayRefund> payRefundRepository,
            IBackgroundJobManager backgroundJobManager,
            ISettingManager settingManager,
            ILogger<PayManager> logger,
            IDistributedCache<QueryOrderCacheItem> queryOrderCache,
            IDistributedCache<AssertNotifyResponse> assertNotifyCache,
            IOptions<PayOptions> payOptions,
            IAbpLazyServiceProvider serviceProvider,
            IObjectMapper objectMapper,
            ICurrentTenantExt currentTenantExt,
            ICommonClient commonClient
            )
        {
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _payParameterRepository = payParameterRepository;
            _payNotifyRepository = payNotifyRepository;
            _payRefundRepository = payRefundRepository;
            _backgroundJobManager = backgroundJobManager;
            _settingManager = settingManager;
            _logger = logger;
            _queryOrderCache = queryOrderCache;
            _assertNotifyCache = assertNotifyCache;
            _payOptions = payOptions;
            _serviceProvider = serviceProvider;
            _objectMapper = objectMapper;
            _currentTenantExt = currentTenantExt;
            _commonClient = commonClient;
        }

        // public async Task<object> Excution(PayTaskType type, string payTaskContent)
        // {
        //     var payOrganizationType = payTaskContent.AsAnonymousObject(new { PayOrganizationType = PayOrganizationType.Unknown });
        //     CheckHelper.IsTrue(payOrganizationType.PayOrganizationType!= PayOrganizationType.Unknown, "付款方式不能为空");

        //     var payProvider = GetPayProvider(payOrganizationType.PayOrganizationType);
        //     payProvider.Excute()
        // }

        /// <summary>
        /// 原路退回
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<PayTaskRefundResult> Refund(RetryRefundArgs args)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.PayOrderId == args.PayOrderId || p.BusinessOrderId == args.PayOrderId || p.PayOrganizationOrderId == args.PayOrderId);
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
                Money = args.Money,
                Status = PayTaskStatus.Executing,
            }, true);

            var request = new RefundPayRequest
            {
                PayParameterValue = payParameter.Value,
                Money = args.Money,
                PayOrderId = payOrder.PayOrderId,
                RefundOrderId = payRefund.RefundOrderId,
            };

            if (args.Index == 0)
            {
                payOrder.RefundStatus = PayTaskStatus.Executing;
                payOrder = await _payOrderRepository.UpdateAsync(payOrder);

                await _payOrderLogRepository.InsertAsync(new PayOrderLog
                {
                    OrderId = payOrder.Id,
                    LogType = OrderLogType.Refund,
                    Status = PayTaskStatus.Executing,
                    OriginRequest = request.ToJsonString(),
                }, true);
            }

            var response = await payProvider.Refund(request);

            var result = new PayTaskRefundResult
            {
                Status = response.Status,
                Message = response.ResponseMessage,
                PayOrderId = args.PayOrderId,
                RefundOrderId = payRefund.RefundOrderId,
            };

            payOrder.RefundStatus = response.Status;
            payOrder.RefundStatusMessage = response.ResponseMessage;
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            payRefund.Status = response.Status;
            payRefund.StatusMessage = response.ResponseMessage;
            payRefund = await _payRefundRepository.UpdateAsync(payRefund);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
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
                //_backgroundJobManager.EnqueueAsync

                return result;
            }

            if (args.Index >= args.Rates.Length - 1)
            {
                payOrder.RefundStatus = PayTaskStatus.Faild;
                payOrder.RefundStatusMessage = $"订单：{args.PayOrderId}发起退款{args.Rates.Length}次仍未成功，放弃退款";
                payOrder = await _payOrderRepository.UpdateAsync(payOrder);

                payRefund.Status = PayTaskStatus.Faild;
                payRefund.StatusMessage = payOrder.QueryStatusMessage;
                payRefund = await _payRefundRepository.UpdateAsync(payRefund);

                await _payOrderLogRepository.InsertAsync(new PayOrderLog
                {
                    OrderId = payOrder.Id,
                    LogType = OrderLogType.Refund,
                    Status = PayTaskStatus.Faild,
                    ExceptionMessage = payOrder.RefundStatusMessage,
                }, true);
            }

            args.Index += 1;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromHours(args.Rates[args.Index]));

            return result;
        }

        /// <summary>
        /// 取消订单
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

            var request = new PayRequest
            {
                PayTaskType = PayTaskType.Cancel,
                PayParameterValue = payParameter.Value,
                PayOrderId = payOrderId,
            };

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                OrderId = payOrder.Id,
                LogType = OrderLogType.Cancel,
                Status = PayTaskStatus.Executing,
                OriginRequest = request.ToJsonString(),
            }, true);

            var response = await payProvider.Cancel(request);

            payOrder.CancelStatus = response.Status;
            payOrder.CancelStatusMessage = response.ResponseMessage;
            payOrder.Status = response.Status;
            await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
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
        /// B2C收款
        /// </summary>
        /// <param name="money"></param>
        /// <param name="authCode"></param>
        /// <param name="targetDomain"></param>
        /// <param name="title"></param>
        /// <param name="ip"></param>
        /// <param name="businessOrderId"></param>
        /// <param name="businessType"></param>
        /// <param name="payRemark"></param>
        /// <returns></returns>
        public async Task<PayTaskSuccessResult> B2C(int money, string authCode, string notifyUrl, string title, string ip, string businessOrderId, BusinessType businessType, string payRemark)
        {
            var paymentType = GetPaymentTypeFromAuthCode(authCode);
            Check.NotNull(paymentType, nameof(paymentType));

            var payParameter = await GetPayParameter(paymentType);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var payOrder = new PayOrder(GuidGenerator.Create())
            {
                TenantId = CurrentTenant.Id,
                Money = money,
                NotifyUrl = notifyUrl,
                Title = title,
                BusinessOrderId = businessOrderId,
                PayOrderId = new PayOrderId
                {
                    PaymentType = paymentType,
                    PayOrganizationType = payParameter.PayOrganizationType,
                    PaymethodType = PaymethodType.B2C,
                    SoftwareCode = _currentTenantExt.SoftwareCode,
                    BusinessType = BusinessType.NoCodePay,
                }.ToString(),
                ParameterVersion = payParameter.Version,
                PaymentType = paymentType,
                PayOrganizationType = payParameter.PayOrganizationType,
                PaymethodType = PaymethodType.B2C,
                BusinessType = businessType,
                Status = PayTaskStatus.Waiting,
                PayRemark = payRemark,
            };
            payOrder = await _payOrderRepository.InsertAsync(payOrder);

            var request = new B2CPayRequest
            {
                AuthCode = authCode,
                NotifyUrl = payOrder.NotifyUrl,
                PayParameterValue = payParameter.Value,
                Money = money,
                Title = title,
                IPAddress = ip,
                PayOrderId = payOrder.PayOrderId,
            };

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                OrderId = payOrder.Id,
                LogType = OrderLogType.Pay,
                Status = PayTaskStatus.Executing,
                OriginRequest = request.ToJsonString(),
            }, true);

            var response = await payProvider.B2C(request);

            var result = new PayTaskSuccessResult
            {
                Status = response.Status,
                Message = response.ResponseMessage,
            };

            payOrder.Status = response.Status;
            payOrder.StatusMessage = response.ResponseMessage;
            payOrder = await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
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
                var rates = GetQueryDelay(QueryDuration);
                await _backgroundJobManager.EnqueueAsync<QueryPayStatusArgs>(new QueryPayStatusArgs
                {
                    TenantId = CurrentTenant.Id.Value,
                    PayOrderId = payOrder.PayOrderId,
                    Rates = rates,
                }, delay: TimeSpan.FromMilliseconds(rates.First()));
            }

            if (response.Status == PayTaskStatus.Success || response.Status == PayTaskStatus.Faild)
            {
                result.PayTime = response.PayTime;
                result.Money = response.Money;
                result.BusinessType = businessType;
                result.PaymentType = paymentType;
                result.BusinessOrderId = businessOrderId;
                result.PayOrderId = payOrder.PayOrderId;
                result.PayOrganizationOrderId = result.PayOrganizationOrderId;

                await _queryOrderCache.RemoveAsync(payOrder.BusinessOrderId);

                var retryNotifyArgs = result.AsObject<RetryNotifyArgs>();
                retryNotifyArgs.NoitfyUrl = notifyUrl;
                await _backgroundJobManager.EnqueueAsync<RetryNotifyArgs>(retryNotifyArgs);
            }

            if (response.Status == PayTaskStatus.Faild)
            {
                await _queryOrderCache.RemoveAsync(payOrder.BusinessOrderId);
            }

            return result;
        }

        /// <summary>
        /// 获取订单状态(缓存)
        /// </summary>
        /// <param name="businessOrderId"></param>
        /// <returns></returns>
        public async Task<PayTaskSuccessResult> GetQueryFromCache(string businessOrderId)
        {
            var queryOrderCacheItem = await _queryOrderCache.GetAsync(businessOrderId);
            if (queryOrderCacheItem.IsNotNull())
            {
                return _objectMapper.Map<QueryOrderCacheItem, PayTaskSuccessResult>(queryOrderCacheItem);
            }

            var payOrder = await _payOrderRepository.FindAsync(p => p.BusinessOrderId == businessOrderId);
            CheckHelper.IsNotNull(payOrder, $"订单：{businessOrderId} 不存在");

            queryOrderCacheItem = _objectMapper.Map<PayOrder, QueryOrderCacheItem>(payOrder);
            await _queryOrderCache.SetAsync(businessOrderId, queryOrderCacheItem);

            return _objectMapper.Map<QueryOrderCacheItem, PayTaskSuccessResult>(queryOrderCacheItem);
        }

        /// <summary>
        /// 获取支付参数
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
            CheckHelper.IsTrue(version.HasValue && version > 0, $"无法确定租户：{CurrentTenant.Id}支付参数的版本号");

            var payParameterValue = await _settingManager.GetOrNullForCurrentTenantAsync(StoreSettings.PayParameterVersionKey, new { Version = version.Value, PaymentType = paymentType });
            if (payParameterValue.IsNotNull()) return payParameterValue.AsObject<PayParameter>();

            var payParameter = await _payParameterRepository.GetPayParameter(paymentType, version);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            await _settingManager.SetForCurrentTenantAsync(StoreSettings.PayParameterVersionKey, payParameter.ToJsonString(), new { Version = payParameter.Version, PaymentType = paymentType });

            return payParameter;
        }

        /// <summary>
        /// 断言回调通知
        /// </summary>
        /// <param name="notify"></param>
        /// <returns></returns>
        public async Task AssertNotify(PayNotify notify)
        {
            try
            {
                notify = await _payNotifyRepository.InsertAsync(notify, autoSave: true);

                var request = new AssertNotifyRequest
                {
                    HashCode = notify.HashCode,
                    PayTaskType = PayTaskType.AssertNotify,
                    PayOrderId = notify.PayOrderId,
                    Method = notify.Method,
                    Url = notify.Url,
                    Body = notify.Body,
                    Query = notify.Query,
                };

                var payOrganizationTypes = PayOrganizationType.GetValues<PayOrganizationType>();

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
                        _logger.LogDebug(ex, $"通道：{payOrganizationType}未实现AssertNotify方法");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"通道：{payOrganizationType}断定通知消息失败，原因：{ex.Message}");
                    }
                }

                CheckHelper.IsNotNull(assertNotifyResults, $"通知内容不合法，原因：未正确解析返回内容");
                if (assertNotifyResults.Count() != 1)
                {
                    _logger.LogCritical($"断定通知结果有多个，无法确定通道通知，命中通道：{assertNotifyResults.Select(p => p.Key).JoinAsString()}，通知内容：{notify.ToJsonString()}");
                    throw new UserFriendlyException($"断定结果有多个，无法确定具体通道通知，命中通道：{assertNotifyResults.Select(p => p.Key).JoinAsString()}");
                }

                var hitAssertNotifyResult = assertNotifyResults.Select(p => p.Value).Single();
                await _assertNotifyCache.SetAsync(hitAssertNotifyResult.PayOrderId.ToString(), hitAssertNotifyResult);

                notify.Status = PayTaskStatus.Success;
                notify.BusinessOrderId = hitAssertNotifyResult.PayOrganizationOrderId;
                notify.PayOrderId = hitAssertNotifyResult.PayOrderId.ToString();
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
        /// 获取支付服务
        /// </summary>
        /// <param name="payOrganizationType"></param>
        /// <returns></returns>
        public IDefaultPayProvider GetPayProvider(PayOrganizationType payOrganizationType)
        {
            var payProviderTypes = _payOptions.Value.PayProviderTypes
                 .Where(p => p.CustomAttributes.Any(c => c.AttributeType == typeof(PayProviderAttribute)))
                 .Where(p => p.CustomAttributes.Any(c => c.ConstructorArguments.Any(a => a.ArgumentType == typeof(PayOrganizationType) && a.Value != null && (PayOrganizationType)a.Value == payOrganizationType)))
                 .ToList();

            CheckHelper.IsNotNull(payProviderTypes, $"支付业务：{payOrganizationType}未实现");
            CheckHelper.AreEqual(payProviderTypes.Count(), 1, message: $"多个支付业务：{payProviderTypes.Select(c => c.Name).JoinAsString()}实现：{payOrganizationType}");

            return (IDefaultPayProvider)_serviceProvider.LazyGetRequiredService(payProviderTypes.Single());
        }

        /// <summary>
        /// 从付款码断定付款方式
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        private PaymentType GetPaymentTypeFromAuthCode(string authCode)
        {
            CheckHelper.IsTrue(int.TryParse(authCode.Substring(0, 2), out var startCode), $"非法条码：{authCode}");

            if (startCode >= 10 && startCode <= 15) // 微信条码支付
            {
                return PaymentType.WechatPay;
            }
            if (startCode >= 20 && startCode <= 33) // 支付宝条码支付
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

            throw new UserFriendlyException($"未知条码：{authCode}");
        }

        /// <summary>
        /// 获取查询速度（毫秒）
        /// </summary>
        /// <param name="delayDuration">轮询时长，单位：分钟</param>
        /// <returns></returns>
        public int[] GetQueryDelay(int delayDuration)
        {
            CheckHelper.IsTrue(delayDuration > 1, $"轮询时长不能小于1分钟");

            var firstMinute = Enumerable.Repeat(3000, 60 * 1000 / 3000);
            var lastMinute = Enumerable.Repeat(5000, (delayDuration - 1) * 60 * 1000 / 5000);

            return firstMinute.Concat(lastMinute).ToArray();
        }

        /// <summary>
        /// 获取退款速度（小时）
        /// </summary>
        /// <param name="delayDuration">轮询时长，单位：天</param>
        /// <returns></returns>
        public int[] GetRefundDelay(int delayDuration)
        {
            CheckHelper.IsTrue(delayDuration > 1, $"轮询时长不能小于1天");

            return Enumerable.Repeat(3, delayDuration * 24 / 3).ToArray();
        }
    }
}
