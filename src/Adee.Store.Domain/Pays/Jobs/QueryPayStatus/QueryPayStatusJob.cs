using System;
using System.Linq;
using System.Threading.Tasks;
using Adee.Store.Domain.Pays;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.ObjectMapping;

namespace Adee.Store.Pays
{
    public class QueryPayStatusJob : BackgroundJob<QueryPayStatusArgs>, ITransientDependency
    {
        private readonly PayManager _payManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IDistributedCache<QueryOrderCacheItem> _queryOrderCache;
        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly ICurrentTenant _currentTenant;

        public QueryPayStatusJob(
            PayManager payManager,
            IBackgroundJobManager backgroundJobManager,
            IDistributedCache<QueryOrderCacheItem> queryOrderCache,
            IRepository<PayOrder> payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            ICurrentTenant currentTenant
            )
        {
            _payManager = payManager;
            _backgroundJobManager = backgroundJobManager;
            _queryOrderCache = queryOrderCache;
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _currentTenant = currentTenant;
        }

        [UnitOfWork(isTransactional: false)]
        public override void Execute(QueryPayStatusArgs args)
        {
            _currentTenant.Change(args.TenantId);

            var executeTask = ExecuteAsync(args);
            executeTask.Wait();
        }

        private async Task ExecuteAsync(QueryPayStatusArgs args)
        {
            await Query(args);
        }

        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task Query(QueryPayStatusArgs args)
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

            var response = await payProvider.Query(request);

            var isCancel = false;
            var stopLoop = false;

            payOrder.QueryStatus = response.Status;
            payOrder.QueryStatusMessage = response.ResponseMessage;

            var payOrderLog = new PayOrderLog
            {
                OrderId = payOrder.Id,
                LogType = OrderLogType.Query,
                Status = payOrder.QueryStatus.Value,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                OriginResponse = response.OriginResponse,
                EncryptResponse = response.EncryptResponse,
                ExceptionMessage = response.ResponseMessage,
            };

            if (response.Status == PayTaskStatus.Success)
            {
                payOrder.PayTime = response.PayTime;
                payOrder.PayOrganizationOrderId = response.PayOrganizationOrderId;
                payOrder.Money = response.Money;
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

                await _queryOrderCache.RemoveAsync(payOrder.BusinessOrderId);

                var retryNotifyArgs = payOrder.AsObject<RetryNotifyArgs>();
                await _backgroundJobManager.EnqueueAsync(retryNotifyArgs);

                if (isCancel)
                {
                    await _payManager.Cancel(args.PayOrderId);
                }

                return;
            }

            args.Index += 1;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromMilliseconds(args.Rates[args.Index]));
        }
    }
}
