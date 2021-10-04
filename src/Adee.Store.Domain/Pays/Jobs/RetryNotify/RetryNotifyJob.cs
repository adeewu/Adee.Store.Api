using System;
using System.Linq;
using System.Threading.Tasks;
using Adee.Store.Pays.Utils.Helpers;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Adee.Store.Pays
{
    public class RetryNotifyJob : StoreTenantBackgroundJob<RetryNotifyArgs>, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly ICommonClient _commonClient;

        public RetryNotifyJob(
            ICurrentTenant currentTenant,
            IBackgroundJobManager backgroundJobManager,
            IRepository<PayOrder> payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            ICommonClient commonClient
            ) : base(currentTenant)
        {
            _currentTenant = currentTenant;
            _backgroundJobManager = backgroundJobManager;
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _commonClient = commonClient;
        }

        [UnitOfWork(isTransactional: false)]
        public override async Task ExecuteAsync(RetryNotifyArgs args)
        {
            using (_currentTenant.Change(args.TenantId))
            {
                await Notify(args);
            }
        }

        private async Task Notify(RetryNotifyArgs args)
        {
            if (args.Index <= 0)
            {
                args.Rates = new int[] { 1, 3, 10, 15, 30, 60, 180, 600, 900, 1800, 3600 };
            }

            var body = args.AsObject<PayTaskSuccessResult>();
            var request = await _commonClient.GetHttpRequestMessage(System.Net.Http.HttpMethod.Post, args.NoitfyUrl, query: null, body: body);
            var response = await _commonClient.GetHttpResponseMessage(request);

            var requestContent = new { url = args.NoitfyUrl, body }.ToJsonString();
            var responseContent = await _commonClient.ReadStringAsync(response);

            if (response.IsSuccessStatusCode)
            {
                await UpdatePayOrderStatus(args.PayOrderId, PayTaskStatus.Success, $"通知成功", requestContent, responseContent);
                return;
            }

            if (args.Index >= args.Rates.Length - 1)
            {
                await UpdatePayOrderStatus(args.PayOrderId, PayTaskStatus.Faild, $"尝试{args.Rates.Length}次通知仍然未成功，放弃通知", requestContent, responseContent);
                return;
            }

            args.Index++;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromSeconds(args.Rates[args.Index]));
        }

        private async Task UpdatePayOrderStatus(string payOrderId, PayTaskStatus status, string message, string request, string response)
        {
            var payOrder = await _payOrderRepository.GetAsync(p => p.PayOrderId == payOrderId);
            if (payOrder == null)
            {
                Logger.LogWarning($"支付订单：{payOrderId}不存在");
                return;
            }

            payOrder.NotifyStatus = status;
            payOrder.NotifyStatusMessage = message;
            await _payOrderRepository.UpdateAsync(payOrder);

            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = payOrder.TenantId,
                OrderId = payOrder.Id,
                LogType = OrderLogType.Notify,
                Status = status,
                StatusMessage = message,
                OriginRequest = request,
                OriginResponse = response,
            }, autoSave: true);
        }
    }
}
