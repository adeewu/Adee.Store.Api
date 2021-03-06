using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Adee.Store.Pays.Jobs
{
    public class RefundQueryStatusJob : StoreTenantBackgroundJob<RefundQueryStatusArgs>, ITransientDependency
    {
        private readonly PayManager _payManager;
        private readonly QueryOrderCacheItemManager _queryOrderCacheItemManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly IRepository<PayRefund> _payRefundRepository;
        private readonly IObjectMapper _objectMapper;

        public RefundQueryStatusJob(
            PayManager payManager,
            QueryOrderCacheItemManager queryOrderCacheItemManager,
            IBackgroundJobManager backgroundJobManager,
            IRepository<PayOrder> payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            IRepository<PayRefund> payRefundRepository,
            ICurrentTenant currentTenant,
            IObjectMapper objectMapper
            ) : base(currentTenant)
        {
            _payManager = payManager;
            _queryOrderCacheItemManager = queryOrderCacheItemManager;
            _backgroundJobManager = backgroundJobManager;
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _payRefundRepository = payRefundRepository;
            _objectMapper = objectMapper;
        }

        [UnitOfWork(isTransactional: false)]
        public override async Task ToExecuteAsync(RefundQueryStatusArgs args)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.PayOrderId == args.PayOrderId);
            CheckHelper.IsNotNull(payOrder, name: nameof(payOrder));

            var refund = await _payRefundRepository.SingleOrDefaultAsync(p => p.Status == PayTaskStatus.Executing && p.OrderId == payOrder.Id);
            CheckHelper.IsNotNull(refund, name: nameof(refund));

            var payParameter = await _payManager.GetPayParameter(payOrder.PaymentType, payOrder.ParameterVersion);
            CheckHelper.IsNotNull(payParameter, name: nameof(payParameter));

            var payProvider = _payManager.GetPayProvider(payOrder.PayOrganizationType);
            CheckHelper.IsNotNull(payProvider, name: nameof(payProvider));

            var request = new PayTaskRequest
            {
                PayTaskType = PayTaskType.RefundQuery,
                PayParameterValue = payParameter.Value,
                PayOrderId = args.PayOrderId,
            };

            if (args.Index == 0)
            {
                if (args.Rates.IsNull())
                {
                    args.Rates = GetQueryDelay(PayConsts.RefundQueryDuration);
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

            SuccessResponse response;
            try
            {
                response = await payProvider.RefundQuery(request);
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
                refund.QueryStatusMessage = $"?????????{args.PayOrderId}??????????????????{args.RefundPayOrderId}??????{args.Rates.Length}????????????????????????????????????";
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
                    StatusMessage = response.ResponseMessage,
                    OriginRequest = response.OriginRequest,
                    SubmitRequest = response.SubmitRequest,
                    OriginResponse = response.OriginResponse,
                    EncryptResponse = response.EncryptResponse,
                };
                await _payOrderLogRepository.InsertAsync(payOrderLog, true);

                await _queryOrderCacheItemManager.RemoveAsync(_objectMapper.Map<PayOrder, QueryOrderCacheItem>(payOrder));

                var result = _objectMapper.Map<PayOrder, OrderResult>(payOrder);
                var notifyArgs = new PayNotifyArgs
                {
                    TenantId = payOrder.TenantId,
                    PayOrderId = payOrder.PayOrderId,
                    NotifyContent = result.ToJsonString(),
                    IsRefundNotify = true,
                };
                await _backgroundJobManager.EnqueueAsync(notifyArgs);

                return;
            }

            args.Index += 1;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromMilliseconds(args.Rates[args.Index]));
        }

        /// <summary>
        /// ??????????????????????????????
        /// </summary>
        /// <param name="delayDuration">??????????????????????????????</param>
        /// <returns></returns>
        public int[] GetQueryDelay(int delayDuration)
        {
            CheckHelper.IsTrue(delayDuration > 1, $"????????????????????????1??????");

            var firstMinute = Enumerable.Repeat(3000, 60 * 1000 / 3000);
            var lastMinute = Enumerable.Repeat(5000, (delayDuration - 1) * 60 * 1000 / 5000);

            return firstMinute.Concat(lastMinute).ToArray();
        }
    }
}
