using Adee.Store.Pays;
using System.Threading.Tasks;

namespace Adee.Store.Domain.Pays
{
    /// <summary>
    /// 支付默认业务
    /// </summary>
    public interface IDefaultPayProvider : IPayProvider
    {
        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaySuccessResponse> Query(PayRequest request);

        /// <summary>
        /// B2C收款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaySuccessResponse> B2C(B2CPayRequest request);

        /// <summary>
        /// C2B收款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PayResponse> C2B(PayRequest request);

        /// <summary>
        /// 断言通知
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AssertNotifyResponse> AssertNotify(AssertNotifyRequest request);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaySuccessResponse> Refund(RefundPayRequest request);

        /// <summary>
        /// 查询退款结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaySuccessResponse> RefundQuery(PayRequest request);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PayResponse> Cancel(PayRequest request);
    }
}
