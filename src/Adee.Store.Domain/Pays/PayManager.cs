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
using Adee.Store;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

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

        public PayManager(
            IRepository<PayOrder> payOrderRepository,
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
            IObjectMapper objectMapper
            )
        {
            _payOrderRepository = payOrderRepository;
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
        }

        // public async Task<object> Excution(PayTaskType type, string payTaskContent)
        // {
        //     var payOrganizationType = payTaskContent.AsAnonymousObject(new { PayOrganizationType = PayOrganizationType.Unknown });
        //     CheckHelper.IsTrue(payOrganizationType.PayOrganizationType!= PayOrganizationType.Unknown, "付款方式不能为空");

        //     var payProvider = GetPayProvider(payOrganizationType.PayOrganizationType);
        //     payProvider.Excute()
        // }

        public async Task<PayResponse> Refund(RetryRefundArgs args)
        {
            CheckHelper.IsTrue(CurrentTenant.IsAvailable, "租户不可用");

            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.TenantId == CurrentTenant.Id && (p.PayOrderId == args.PayOrderId || p.BusinessOrderId == args.PayOrderId || p.PayOrganizationOrderId == args.PayOrderId));
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            var payRefund = await _payRefundRepository.InsertAsync(new PayRefund(GuidGenerator.Create())
            {
                TenantId = CurrentTenant.Id,
                OrderId = payOrder.Id,
                RefundOrderId = $"Refund-{payOrder.PayOrderId}-{new Random(GetHashCode()).Next(100, 999)}",
                Money = args.Money,
                Status = PayTaskStatus.Normal,
            }, true);

            var payParameter = await GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            payRefund.Status = PayTaskStatus.Executing;
            payRefund = await _payRefundRepository.UpdateAsync(payRefund, true);

            var result = await payProvider.Refund(new RefundPayRequest
            {
                TenantId = CurrentTenant.Id.Value,
                PayParameterValue = payParameter.Value,
                Money = args.Money,
                PayOrderId = payOrder.PayOrderId,
                RefundOrderId = payRefund.RefundOrderId,
            });

            payOrder.RefundStatus = result.Status;
            payOrder.RefundStatusMessage = result.ResponseMessage;

            payRefund.Status = result.Status;
            payRefund.StatusMessage = result.ResponseMessage;
            await _payRefundRepository.UpdateAsync(payRefund, true);

            if (result.Status == PayTaskStatus.Normal || result.Status == PayTaskStatus.Executing || result.Status == PayTaskStatus.Waiting)
            {
                args.Index += 1;

                if (args.Index >= args.Rates.Length)
                {
                    payRefund.Status = PayTaskStatus.Faild;
                    payRefund.StatusMessage = payOrder.QueryStatusMessage;
                    payOrder.RefundStatus = PayTaskStatus.Faild;
                    payOrder.RefundStatusMessage = $"订单：{args.PayOrderId}发起退款{args.Rates.Length}次仍未成功，放弃退款";
                    payOrder = await _payOrderRepository.UpdateAsync(payOrder);
                }
                else
                {
                    await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromHours(args.Rates[args.Index]));
                }
            }

            return result;
        }

        public async Task Cancel(string payOrderId)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.TenantId == CurrentTenant.Id && p.PayOrderId == payOrderId);
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            var payParameter = await GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var result = await payProvider.Cancel(new PayRequest
            {
                TenantId = CurrentTenant.Id.Value,
                PayTaskType = PayTaskType.Cancel,
                PayParameterValue = payParameter.Value,
                PayOrderId = payOrderId,
            });

            payOrder.CancelStatus = result.Status;
            payOrder.CancelStatusMessage = result.ResponseMessage;
            await _payOrderRepository.UpdateAsync(payOrder, true);
        }

        public async Task<PaySuccessResponse> B2C(int money, string authCode, string targetDomain, string title, string ip, string businessOrderId, BusinessType businessType, string payRemark)
        {
            CheckHelper.IsTrue(CurrentTenant.IsAvailable, "租户不可用");

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
                TargetDomain = targetDomain,
                NotifyUrl = $"{targetDomain}/api/app/notify/{CurrentTenant.Id}",
                Title = title,
                BusinessOrderId = businessOrderId,
                PayOrderId = new PayOrderId
                {
                    PaymentType = paymentType,
                    PayOrganizationType = payParameter.PayOrganizationType,
                    PaymethodType = PaymethodType.B2C,
                    SoftwareCode = "",
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
            payOrder = await _payOrderRepository.InsertAsync(payOrder, true);

            var result = await payProvider.B2C(new B2CPayRequest
            {
                AuthCode = authCode,
                TargetDomain = targetDomain,
                TenantId = CurrentTenant.Id.Value,
                PayParameterValue = payParameter.Value,
                Money = money,
                Title = title,
                IPAddress = ip,
                PayOrderId = payOrder.PayOrderId,
            });

            payOrder.Status = result.Status;
            payOrder.StatusMessage = result.ResponseMessage;
            await _payOrderRepository.UpdateAsync(payOrder, true);

            if (result.Status == PayTaskStatus.Normal || result.Status == PayTaskStatus.Executing || result.Status == PayTaskStatus.Waiting)
            {
                var rates = GetQueryDelay(QueryDuration);
                await _backgroundJobManager.EnqueueAsync<QueryPayStatusArgs>(new QueryPayStatusArgs
                {
                    TenantId = CurrentTenant.Id.Value,
                    PayParameterVersion = payParameter.Version,
                    PayOrderId = payOrder.PayOrderId,
                    Rates = rates,
                }, delay: TimeSpan.FromMilliseconds(rates.First()));
            }

            if (result.Status == PayTaskStatus.Success || result.Status == PayTaskStatus.Faild)
            {
                await _queryOrderCache.RemoveAsync(payOrder.BusinessOrderId);
            }

            return result;
        }

        public async Task<QueryOrderCacheItem> QueryCacheItem(string businessOrderId)
        {
            var response = await _queryOrderCache.GetAsync(businessOrderId);
            if (response.IsNotNull()) return response;

            var payOrder = await _payOrderRepository.FindAsync(p => p.PayOrderId == businessOrderId);
            CheckHelper.IsNotNull(payOrder, $"订单：{businessOrderId} 不存在");
            CheckHelper.IsTrue(payOrder.TenantId.HasValue, $"订单：{businessOrderId}的租户Id不能为空");

            response = _objectMapper.Map<PayOrder, QueryOrderCacheItem>(payOrder);
            await _queryOrderCache.SetAsync(businessOrderId, response);

            return response;
        }

        public async Task<PaySuccessResponse> Query(QueryPayStatusArgs args)
        {
            CheckHelper.IsTrue(CurrentTenant.IsAvailable, "租户不可用");

            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.TenantId == CurrentTenant.Id && p.PayOrderId == args.PayOrderId);
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            if (args.Index == 0)
            {
                payOrder.QueryStatus = PayTaskStatus.Executing;
                payOrder = await _payOrderRepository.UpdateAsync(payOrder, true);
            }

            var payParameter = await GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = GetPayProvider(payParameter.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var result = await payProvider.Query(new PayRequest
            {
                TenantId = CurrentTenant.Id.Value,
                PayTaskType = PayTaskType.Query,
                PayParameterValue = payParameter.Value,
                PayOrderId = args.PayOrderId,
            });

            payOrder.QueryStatus = result.Status;
            payOrder.QueryStatusMessage = result.ResponseMessage;

            if (result.Status == PayTaskStatus.Success)
            {
                payOrder.PayTime = result.PayTime;
                payOrder.PayOrganizationOrderId = result.PayOrganizationOrderId;
                payOrder.Money = result.Money;
                payOrder.Status = result.Status;

                await _queryOrderCache.RemoveAsync(payOrder.BusinessOrderId);
            }

            payOrder = await _payOrderRepository.UpdateAsync(payOrder, true);

            if (result.Status == PayTaskStatus.Success || result.Status == PayTaskStatus.Faild)
            {
                await _queryOrderCache.RemoveAsync(payOrder.BusinessOrderId);
            }

            if (result.Status == PayTaskStatus.Normal || result.Status == PayTaskStatus.Executing || result.Status == PayTaskStatus.Waiting)
            {
                args.Index += 1;

                if (args.Index >= args.Rates.Length)
                {
                    payOrder.QueryStatus = PayTaskStatus.Faild;
                    payOrder.QueryStatusMessage = $"订单：{args.PayOrderId}轮询{args.Rates.Length}次仍未取到结果，放弃轮询";
                    payOrder.Status = PayTaskStatus.Faild;
                    payOrder.StatusMessage = payOrder.QueryStatusMessage;
                    payOrder = await _payOrderRepository.UpdateAsync(payOrder, true);

                    await Cancel(args.PayOrderId);
                }
                else
                {
                    await _backgroundJobManager.EnqueueAsync<QueryPayStatusArgs>(args, delay: TimeSpan.FromMilliseconds(args.Rates[args.Index]));
                }
            }

            return result;
        }

        public async Task<PayParameter> GetPayParameter(PaymentType paymentType, long? version = null)
        {
            var isSetVersion = false;
            if (version.HasValue == false)
            {
                var setVersion = (await _settingManager.GetOrNullForCurrentTenantAsync(StoreSettings.PayParameterVersionKey)).To(0L);
                if (setVersion > 0)
                {
                    isSetVersion = true;
                    version = setVersion;
                }
                else
                {
                    version = await _payParameterRepository.GetMaxPayParameterVersion(CurrentTenant.Id);
                }
            }
            CheckHelper.IsTrue(version.HasValue && version > 0, $"无法确定租户：{CurrentTenant.Id}支付参数的版本号");

            var payParameterValue = await _settingManager.GetOrNullForCurrentTenantAsync(StoreSettings.PayParameterVersionKey, new { Version = version.Value, PaymentType = paymentType });
            if (payParameterValue.IsNotNull()) return payParameterValue.AsObject<PayParameter>();

            var payParameter = await _payParameterRepository.GetPayParameter(CurrentTenant.Id, paymentType, version);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            if (isSetVersion == false)
            {
                await _settingManager.SetForCurrentTenantAsync(StoreSettings.PayParameterVersionKey, payParameter.Version.ToString());
            }
            await _settingManager.SetForCurrentTenantAsync(StoreSettings.PayParameterVersionKey, payParameter.ToJsonString(), new { Version = payParameter.Version, PaymentType = paymentType });

            return payParameter;
        }

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
                notify.MerchantOrderId = hitAssertNotifyResult.PayOrganizationOrderId;
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

            return firstMinute.Union(lastMinute).ToArray();
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
