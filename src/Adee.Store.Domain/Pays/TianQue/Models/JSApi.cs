namespace Adee.Store.Domain.Pays.TianQue.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class JSApiRequestModel : Request
    {
        /// <summary>
        /// Required String 支付方式，枚举值 取值范围：02 微信公众号/支付宝服务窗/银联js支付/支付宝小程序 03 微信小程序
        /// </summary>
        public string payWay { get; set; }

        /// <summary>
        /// Optional String 微信扫码点餐标识，最大长度32位 注：目前需上送：FoodOrder
        /// </summary>
        public string wechatFoodOrder { get; set; }

        /// <summary>
        /// Optional String 支付宝扫码点餐类型，最大长度128位 扫码点餐标识: qr_order 店内扫码点餐 pre_order 预点到店自提 home_delivery 外送到家 direct_payment 直接付款 other 其他 支付宝扫码点餐上传这个字段,上传菜品调用官方接口
        /// </summary>
        public string alipayFoodOrderType { get; set; }

        /// <summary>
        /// Optional String 订单失效时间（单位分钟），格式：#### 取值范围：1-1440，默认5分钟
        /// </summary>
        public string timeExpire { get; set; }

        /// <summary>
        /// Conditional String 持卡人ip地址，最大长度40位 银联js支付时，该参数必传
        /// </summary>
        public string customerIp { get; set; }

        /// <summary>
        /// Conditional String 用户号（微信openid/支付宝userid/银联userid），最大长度128位 payType=="WECHAT"||payType=="ALIPAY"时必传
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// Conditional String 银联js支付成功前端跳转地址，最大长度128位 与成功地址/失败地址同时存在或同时不存在
        /// </summary>
        public string outFrontUrl { get; set; }

        /// <summary>
        /// Conditional String 银联js支付失败前端跳转地址，最大长度128位 与成功地址/失败地址同时存在或同时不存在
        /// </summary>
        public string outFrontFailUrl { get; set; }

        /// <summary>
        /// Optional String 银联扫码点餐-门店标识，长度288位
        /// </summary>
        public string addnInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JSApiResponseModel : Response
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
        /// Optional String 微信预下单id
        /// </summary>
        public string prepayId { get; set; }

        /// <summary>
        /// Optional String 微信 AppId
        /// </summary>
        public string payAppId { get; set; }

        /// <summary>
        /// Optional String 微信 TimeStamp
        /// </summary>
        public string payTimeStamp { get; set; }

        /// <summary>
        /// Optional String 微信 NonceStr
        /// </summary>
        public string paynonceStr { get; set; }

        /// <summary>
        /// Optional String 微信 Package
        /// </summary>
        public string payPackage { get; set; }

        /// <summary>
        /// Optional String 微信 SignType
        /// </summary>
        public string paySignType { get; set; }

        /// <summary>
        /// Optional String 微信 Sign
        /// </summary>
        public string paySign { get; set; }

        /// <summary>
        /// Optional String 微信 PartnerId
        /// </summary>
        public string partnerId { get; set; }

        /// <summary>
        /// Conditional String 银联重定向跳转地址
        /// </summary>
        public string redirectUrl { get; set; }

        /// <summary>
        /// Conditional String 支付宝流水号或支付宝支付链接 服务商需做判断，若返回为流水号需将流水号拼装为链接调起支付控件，若为支付链接则可拿链接调起支付控件（对应trade_no）
        /// </summary>
        public string source { get; set; }
    }
}
