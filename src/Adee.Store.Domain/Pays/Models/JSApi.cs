namespace Adee.Store.Pays
{
    /// <summary>
    /// JS支付
    /// </summary>
    public class JSApi : C2B
    {
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
    }
}
