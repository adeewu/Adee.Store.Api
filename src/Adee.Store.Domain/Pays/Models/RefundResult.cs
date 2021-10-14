namespace Adee.Store.Pays
{
    /// <summary>
    /// 退款结果
    /// </summary>
    public class RefundResult : PayTaskResult
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 业务订单号
        /// </summary>
        public string BusinessOrderId { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundOrderId { get; set; }
    }
}
