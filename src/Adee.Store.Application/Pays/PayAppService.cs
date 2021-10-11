using Adee.Store.Attributes;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付服务
    /// </summary>    
    [ApiGroup(ApiGroupType.Pay)]
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
        /// <param name="orderId">支持BusinessOrderId、PayOrderId、PayOrganizationOrderId</param>
        /// <returns></returns>
        public async Task<PayTaskOrderResult> GetQuery(string orderId)
        {
            return await _payManager.GetQueryFromCache(orderId);
        }

        /// <summary>
        /// B2C收款
        /// </summary>
        /// <returns></returns>
        public async Task<PayTaskOrderResult> B2C(B2CPayTaskDto dto)
        {
            var model = ObjectMapper.Map<B2CPayTaskDto, B2C>(dto);

            return await _payManager.B2C(model);
        }

        /// <summary>
        /// 原路退回
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PayTaskRefundResult> Refund(RefundPayTaskDto dto)
        {
            return await _payManager.Refund(dto.OrderId, dto.Money);
        }
    }
}
