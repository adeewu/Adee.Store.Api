namespace Adee.Store.Domain.Pays.TianQue.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class C2BRequestModel : Request
    {
        /// <summary>
        /// Optional String 订单失效时间（单位分钟），格式：#### 取值范围：1-1440，默认5分钟
        /// </summary>
        public string timeExpire { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class C2BResponseModel : Response
    {
        /// <summary>
        /// Required String 商户订单号，长度64位
        /// </summary>
        public string ordNo { get; set; }

        /// <summary>
        /// Required String 天阙平台订单号，长度32位
        /// </summary>
        public string uuid { get; set; }

        /// <summary>
        /// Conditional String 落单号，长度32位 供退款和退款查询使用。
        /// </summary>
        public string sxfUuid { get; set; }

        /// <summary>
        /// Required String 包含订单信息的二维码链接，最大长度1024位 商户通过该链接生成二维码供用户扫码支付
        /// </summary>
        public string payUrl { get; set; }
    }
}
