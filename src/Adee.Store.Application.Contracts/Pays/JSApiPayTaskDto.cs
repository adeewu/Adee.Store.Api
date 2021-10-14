namespace Adee.Store.Pays
{
    /// <summary>
    /// JSApi收款任务
    /// </summary>
    public class JSApiPayTaskDto : BasePayTaskDto
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 支付端形式
        /// </summary>
        public JSApiTradeType TradeType { get; set; }

        /// <summary>
        /// 微信OpenId，支付宝UserId，银联UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 应用Id，仅限微信小程序、微信公众号
        /// </summary>
        public string SubAppId { get; set; }

        /// <summary>
        /// 支付超时时间，单位：分钟
        /// </summary>
        public int PayExpire { get; set; }
    }
}
