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
        public async Task<OrderResult> Query(string orderId)
        {
            return await _payManager.GetQueryFromCache(orderId);
        }

        /// <summary>
        /// B2C收款
        /// </summary>
        /// <returns></returns>
        public async Task<OrderResult> B2c(B2CPayTaskDto dto)
        {
            var model = ObjectMapper.Map<B2CPayTaskDto, B2C>(dto);

            return await _payManager.B2C(model);
        }

        /// <summary>
        /// C2B收款
        /// </summary>
        /// <returns></returns>
        public async Task<PayUrlResult> C2b(C2BPayTaskDto dto)
        {
            var model = ObjectMapper.Map<C2BPayTaskDto, C2B>(dto);

            return await _payManager.C2B(model);
        }

        /// <summary>
        /// JSApi收款
        /// </summary>
        /// <returns></returns>
        public async Task<ParameterResult> Jsapi(JSApiPayTaskDto dto)
        {
            var model = ObjectMapper.Map<JSApiPayTaskDto, JSApi>(dto);

            return await _payManager.JSApi(model);
        }

        /// <summary>
        /// 原路退回
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<RefundResult> Refund(RefundPayTaskDto dto)
        {
            return await _payManager.Refund(dto.OrderId, dto.Money);
        }
    }
}
