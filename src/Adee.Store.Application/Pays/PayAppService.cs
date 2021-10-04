using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付服务(内部用)
    /// </summary>    
    public class PayAppService : StoreWithRequestAppService
    {
        private readonly PayManager _payManager;

        public PayAppService(
            PayManager payManager
            )
        {
            _payManager = payManager;
        }

        // public async Task<object> PayTask(PayTaskContentDto dto)
        // {
        //     return await _payManager.Excution(dto.PayTaskType, dto.Content);
        // }

        /// <summary>
        /// 查询支付订单状态
        /// </summary>
        /// <param name="businessOrderId"></param>
        /// <returns></returns>
        public async Task<PayTaskSuccessResult> GetQuery(string businessOrderId)
        {
            return await _payManager.GetQueryFromCache(businessOrderId);
        }

        /// <summary>
        /// B2C收款
        /// </summary>
        /// <returns></returns>
        public async Task<PayStatusResultDto> B2C(B2CPayTaskDto dto)
        {
            var businessOrderId = new BusinessOrderId { SoftwareCode = CurrentTenantExt.SoftwareCode };

            var notifyUrl = GetNotifyUrl(CurrentDomain, CurrentTenant.Id);
            var result = await _payManager.B2C(dto.Money, dto.AuthCode, notifyUrl, dto.Title, dto.IPAddress, businessOrderId.ToString(), dto.businessType, dto.PayRemark);

            return new PayStatusResultDto
            {
                BusinessOrderId = businessOrderId.ToString(),
                Status = result.Status,
                Message = result.Message,
            };
        }

        /// <summary>
        /// 原路退回
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PayStatusResultDto> Refund(RefundPayTaskDto dto)
        {
            var result = await _payManager.Refund(new RetryRefundArgs
            {
                TenantId = CurrentTenant.Id.Value,
                Money = dto.Money,
                PayOrderId = dto.OrderId,
                Rates = _payManager.GetRefundDelay(PayManager.RefundDuration)
            });

            return new PayStatusResultDto
            {
                BusinessOrderId = dto.OrderId,
                Status = result.Status,
            };
        }

        /// <summary>
        /// 获取通知地址
        /// </summary>
        /// <param name="targetDomain"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private string GetNotifyUrl(string targetDomain, Guid? tenantId)
        {
            var targetTenantId = Guid.Empty;
            if (tenantId.HasValue) targetTenantId = tenantId.Value;

            return $"{targetDomain}/api/app/notify?__tenant={targetTenantId}";
        }
    }
}
