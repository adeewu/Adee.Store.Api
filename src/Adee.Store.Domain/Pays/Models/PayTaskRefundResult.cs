namespace Adee.Store.Pays
{
    /// <summary>
    /// 退款结果
    /// </summary>
    public class PayTaskRefundResult : PayTaskResult
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundOrderId { get; set; }
    }
}
