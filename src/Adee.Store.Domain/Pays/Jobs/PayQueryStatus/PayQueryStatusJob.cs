using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Adee.Store.Pays
{
    public class PayQueryStatusJob : StoreTenantBackgroundJob<PayQueryStatusArgs>, ITransientDependency
    {
        private readonly PayManager _payManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly QueryOrderCacheItemManager _queryOrderCacheItemManager;
        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly IObjectMapper _objectMapper;

        public PayQueryStatusJob(
            PayManager payManager,
            IBackgroundJobManager backgroundJobManager,
            QueryOrderCacheItemManager queryOrderCacheItemManager,
            IRepository<PayOrder> payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            ICurrentTenant currentTenant,
            IObjectMapper objectMapper
            ) : base(currentTenant)
        {
            _payManager = payManager;
            _backgroundJobManager = backgroundJobManager;
            _queryOrderCacheItemManager = queryOrderCacheItemManager;
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _objectMapper = objectMapper;
        }

        [UnitOfWork(isTransactional: false)]
        public override async Task ExecuteAsync(PayQueryStatusArgs args)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.PayOrderId == args.PayOrderId);
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            var payParameter = await _payManager.GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = _payManager.GetPayProvider(payOrder.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var request = new PayRequest
            {
                PayTaskType = PayTaskType.Query,
                PayParameterValue = payParameter.Value,
                PayOrderId = args.PayOrderId,
            };

            if (args.Index == 0)
            {
                if (args.Rates.IsNull())
                {
                    args.Rates = GetQueryDelay(PayConsts.QueryDuration);
                }

                payOrder.QueryStatus = PayTaskStatus.Executing;
                payOrder = await _payOrderRepository.UpdateAsync(payOrder);

                await _payOrderLogRepository.InsertAsync(new PayOrderLog
                {
                    OrderId = payOrder.Id,
                    LogType = OrderLogType.Query,
                    Status = PayTaskStatus.Executing,
                    OriginRequest = request.ToJsonString(),
                }, true);
            }

            PaySuccessResponse response;
            try
            {
                response = await payProvider.Query(request);
            }
            catch (Exception ex)
            {
                response = new PaySuccessResponse
                {
                    Status = PayTaskStatus.Faild,
                    ResponseMessage = ex.Message,
                    OriginRequest = request.ToJsonString(),
                    OriginResponse = ex.ToJsonString(),
                };
            }

            var isCancel = false;
            var stopLoop = false;

            payOrder.QueryStatus = response.Status;
            payOrder.QueryStatusMessage = response.ResponseMessage;

            var payOrderLog = new PayOrderLog
            {
                OrderId = payOrder.Id,
                LogType = OrderLogType.Query,
                Status = payOrder.QueryStatus.Value,
                StatusMessage = response.ResponseMessage,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                OriginResponse = response.OriginResponse,
            };

            if (response.Status == PayTaskStatus.Success)
            {
                payOrder.PayTime = response.PayTime;
                payOrder.PayOrganizationOrderId = response.PayOrganizationOrderId;
                if (response.Money.HasValue)
                {
                    payOrder.Money = response.Money.Value;
                }
                payOrder.Status = response.Status;

                stopLoop = true;
            }

            if (response.Status == PayTaskStatus.Faild)
            {
                payOrder.Status = response.Status;

                stopLoop = true;
            }

            if (args.Index >= args.Rates.Length - 1)
            {
                payOrder.QueryStatus = PayTaskStatus.Faild;
                payOrder.QueryStatusMessage = $"订单：{args.PayOrderId}轮询{args.Rates.Length}次仍未取到结果，放弃轮询";
                payOrder.Status = PayTaskStatus.Faild;

                isCancel = true;
                stopLoop = true;
            }

            if (stopLoop)
            {
                payOrder = await _payOrderRepository.UpdateAsync(payOrder);

                await _payOrderLogRepository.InsertAsync(payOrderLog, true);

                await _queryOrderCacheItemManager.RemoveAsync(_objectMapper.Map<PayOrder, QueryOrderCacheItem>(payOrder));

                var result = _objectMapper.Map<PayOrder, PayTaskOrderResult>(payOrder);
                var notifyArgs = new PayNotifyArgs
                {
                    TenantId = payOrder.TenantId,
                    PayOrderId = payOrder.PayOrderId,
                    NotifyContent = result.ToJsonString(),
                };
                await _backgroundJobManager.EnqueueAsync(notifyArgs);

                if (isCancel)
                {
                    await _payManager.Cancel(args.PayOrderId);
                }

                return;
            }

            args.Index += 1;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromMilliseconds(args.Rates[args.Index]));
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
    }
}
