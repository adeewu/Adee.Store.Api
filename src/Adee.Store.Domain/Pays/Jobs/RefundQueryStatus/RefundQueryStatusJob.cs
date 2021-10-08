using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Adee.Store.Pays
{
    public class RefundQueryStatusJob : StoreTenantBackgroundJob<RefundQueryStatusArgs>, ITransientDependency
    {
        private readonly PayManager _payManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IDistributedCache<QueryOrderCacheItem> _queryOrderCache;
        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly IRepository<PayRefund> _payRefundRepository;

        public RefundQueryStatusJob(
            PayManager payManager,
            IBackgroundJobManager backgroundJobManager,
            IDistributedCache<QueryOrderCacheItem> queryOrderCache,
            IRepository<PayOrder> payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            IRepository<PayRefund> payRefundRepository,
            ICurrentTenant currentTenant
            ) : base(currentTenant)
        {
            _payManager = payManager;
            _backgroundJobManager = backgroundJobManager;
            _queryOrderCache = queryOrderCache;
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _payRefundRepository = payRefundRepository;
        }

        [UnitOfWork(isTransactional: false)]
        public override async Task ExecuteAsync(RefundQueryStatusArgs args)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.PayOrderId == args.PayOrderId);
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            var refund = await _payRefundRepository.SingleOrDefaultAsync(p => p.Status == PayTaskStatus.Executing && p.OrderId == payOrder.Id);
            CheckHelper.IsNotNull(refund, name: nameof(refund));

            var payParameter = await _payManager.GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = _payManager.GetPayProvider(payOrder.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var request = new PayRequest
            {
                PayTaskType = PayTaskType.RefundQuery,
                PayParameterValue = payParameter.Value,
                PayOrderId = args.PayOrderId,
            };

            if (args.Index == 0)
            {
                if (args.Rates.IsNull())
                {
                    args.Rates = GetRefundDelay(PayManager.RefundQueryDuration);
                }

                refund.QueryStatus = PayTaskStatus.Executing;
                refund = await _payRefundRepository.UpdateAsync(refund);

                await _payOrderLogRepository.InsertAsync(new PayOrderLog
                {
                    OrderId = payOrder.Id,
                    LogType = OrderLogType.RefundQuery,
                    Status = PayTaskStatus.Executing,
                    OriginRequest = request.ToJsonString(),
                }, true);
            }

            var response = await payProvider.RefundQuery(request);

            var stopLoop = false;

            refund.QueryStatus = response.Status;
            refund.QueryStatusMessage = response.ResponseMessage;

            if (response.Status == PayTaskStatus.Success)
            {
                refund.Status = response.Status;
                payOrder.RefundStatus = refund.Status;

                stopLoop = true;
            }

            if (response.Status == PayTaskStatus.Faild)
            {
                refund.Status = response.Status;
                payOrder.RefundStatus = refund.Status;

                stopLoop = true;
            }

            if (args.Index >= args.Rates.Length - 1)
            {
                refund.QueryStatus = PayTaskStatus.Faild;
                refund.QueryStatusMessage = $"订单：{args.PayOrderId}的退款订单：{args.RefundPayOrderId}轮询{args.Rates.Length}次仍未取到结果，放弃轮询";
                refund.Status = PayTaskStatus.Faild;
                payOrder.RefundStatus = refund.Status;

                stopLoop = true;
            }

            if (stopLoop)
            {
                payOrder = await _payOrderRepository.UpdateAsync(payOrder);

                refund = await _payRefundRepository.UpdateAsync(refund);

                var payOrderLog = new PayOrderLog
                {
                    OrderId = payOrder.Id,
                    LogType = OrderLogType.RefundQuery,
                    Status = refund.QueryStatus.Value,
                    OriginRequest = response.OriginRequest,
                    SubmitRequest = response.SubmitRequest,
                    OriginResponse = response.OriginResponse,
                    EncryptResponse = response.EncryptResponse,
                    ExceptionMessage = response.ResponseMessage,
                };
                await _payOrderLogRepository.InsertAsync(payOrderLog, true);

                await _queryOrderCache.RemoveAsync(payOrder.BusinessOrderId);

                var notifyArgs = new PayNotifyArgs
                {
                    TenantId = payOrder.TenantId,
                    PayOrderId = payOrder.PayOrderId,
                    NotifyContent = payOrder,
                };
                await _backgroundJobManager.EnqueueAsync(notifyArgs);

                return;
            }

            args.Index += 1;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromMilliseconds(args.Rates[args.Index]));
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
