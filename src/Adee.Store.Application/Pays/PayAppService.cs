using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付服务
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
        public async Task<QueryOrderCacheItem> GetQuery(string businessOrderId)
        {
            return await _payManager.QueryCacheItem(businessOrderId);
        }

        /// <summary>
        /// B2C收款
        /// </summary>
        /// <returns></returns>
        public async Task<PayStatusResultDto> B2C(B2CPayTaskDto dto)
        {
            var softwareCode = await CurrentTenantExt.GetSoftwareCodeAsync();
            var businessOrderId = new BusinessOrderId { SoftwareCode = softwareCode };

            var result = await _payManager.B2C(dto.Money, dto.AuthCode, CurrentDomain, dto.Title, dto.IPAddress, businessOrderId.ToString(), dto.businessType, dto.PayRemark);

            return new PayStatusResultDto
            {
                BusinessOrderId = businessOrderId.ToString(),
                Status = result.Status,
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
    }
}
