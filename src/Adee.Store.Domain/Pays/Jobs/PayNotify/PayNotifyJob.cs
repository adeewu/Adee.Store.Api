using Adee.Store.Pays.Utils.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Adee.Store.Pays.Jobs
{
    public class PayNotifyJob : StoreTenantBackgroundJob<PayNotifyArgs>, ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IRepository<PayOrderLog> _payOrderLogRepository;
        private readonly IRepository<PayRefund> _payRefundRepository;
        private readonly ICommonClient _commonClient;

        public PayNotifyJob(
            ICurrentTenant currentTenant,
            IBackgroundJobManager backgroundJobManager,
            IRepository<PayOrder> payOrderRepository,
            IRepository<PayOrderLog> payOrderLogRepository,
            IRepository<PayRefund> payRefundRepository,
            ICommonClient commonClient
            ) : base(currentTenant)
        {
            _backgroundJobManager = backgroundJobManager;
            _payOrderRepository = payOrderRepository;
            _payOrderLogRepository = payOrderLogRepository;
            _payRefundRepository = payRefundRepository;
            _commonClient = commonClient;
        }

        [UnitOfWork(isTransactional: false)]
        public override async Task ToExecuteAsync(PayNotifyArgs args)
        {
            var payOrder = await _payOrderRepository.SingleOrDefaultAsync(p => p.PayOrderId == args.PayOrderId);
            CheckHelper.IsNotNull(payOrder, $"支付订单：{args.PayOrderId}不存在");

            var requestContent = new
            {
                url = payOrder.NotifyUrl,
                body = args.NotifyContent
            }.ToJsonString();

            if (args.Index <= 0)
            {
                if (args.Rates.IsNull())
                {
                    args.Rates = new int[] { 1, 3, 10, 15, 30, 60, 180, 600, 900, 1800, 3600 };
                }

                await UpdatePayOrderStatus(payOrder, PayTaskStatus.Executing, $"开始通知", requestContent, null, args.IsRefundNotify);
            }

            var responseContent = string.Empty;

            try
            {
                var request = await _commonClient.GetHttpRequestMessage(System.Net.Http.HttpMethod.Post, payOrder.NotifyUrl, query: null, body: args.NotifyContent);
                var response = await _commonClient.GetHttpResponseMessage(request);

                responseContent = await _commonClient.ReadStringAsync(response);

                await UpdatePayOrderStatus(payOrder, PayTaskStatus.Success, $"通知成功", null, responseContent, args.IsRefundNotify);
                return;
            }
            catch (Exception ex)
            {
                var errorMessage = $"支付订单：{args.PayOrderId}在第{args.Index}次通知发生错误，原因：{ex.Message}";
                Logger.LogError(ex, errorMessage);
                if (args.Index == 0)
                {
                    await UpdatePayOrderLogStatus(payOrder, PayTaskStatus.Faild, errorMessage, requestContent, null, args.IsRefundNotify);
                }
            }

            if (args.Index >= args.Rates.Length - 1)
            {
                await UpdatePayOrderStatus(payOrder, PayTaskStatus.Faild, $"尝试{args.Rates.Length}次通知仍然未成功，放弃通知", requestContent, responseContent, args.IsRefundNotify);
                return;
            }

            args.Index++;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromSeconds(args.Rates[args.Index]));
        }

        private async Task UpdatePayOrderStatus(PayOrder payOrder, PayTaskStatus status, string message, string request, string response, bool isRefundNotify)
        {
            if (isRefundNotify)
            {
                var refund = await _payRefundRepository.SingleOrDefaultAsync(p => p.OrderId == payOrder.Id);
                CheckHelper.IsNotNull(payOrder, $"支付订单：{payOrder.PayOrderId}没有退款订单");

                refund.NotifyStatus = status;
                refund.NotifyStatusMessage = message;

                await _payRefundRepository.UpdateAsync(refund);
            }
            else
            {
                payOrder.NotifyStatus = status;
                payOrder.NotifyStatusMessage = message;

                await _payOrderRepository.UpdateAsync(payOrder);
            }

            await UpdatePayOrderLogStatus(payOrder, status, message, request, response, isRefundNotify);
        }

        private async Task UpdatePayOrderLogStatus(PayOrder payOrder, PayTaskStatus status, string message, string request, string response, bool isRefundNotify)
        {
            await _payOrderLogRepository.InsertAsync(new PayOrderLog
            {
                TenantId = payOrder.TenantId,
                OrderId = payOrder.Id,
                LogType = isRefundNotify ? OrderLogType.RefundNotify : OrderLogType.Notify,
                Status = status,
                StatusMessage = message,
                OriginRequest = request,
                OriginResponse = response,
            }, autoSave: true);
        }
    }
}
