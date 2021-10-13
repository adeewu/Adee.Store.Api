using System;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付参数任务结果
    /// </summary>
    public class ParameterResult : PayTaskResult
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 收单机构订单号
        /// </summary>
        public string PayOrganizationOrderId { get; set; }

        /// <summary>
        /// 参数，具体格式需要参考PaymentType+JSApiTradeType
        /// </summary>
        public string Parameter { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WechatJSApiResult
    {
        /// <summary>
        /// 微信分配的 APPID 参考微信对应的调起支付 API
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 时间戳 参考微信对应的调起支付 API
        /// </summary>
        public string timeStamp { get; set; }

        /// <summary>
        /// 随机串 参考微信对应的调起支付 API
        /// </summary>
        public string nonceStr { get; set; }

        /// <summary>
        /// 数据包 参考微信对应的调起支付 API
        /// </summary>
        public string package { get; set; }

        /// <summary>
        /// 签名方式 参考微信对应的调起支付 API
        /// </summary>
        public string signType { get; set; }

        /// <summary>
        /// 签名数据 参考微信对应的调起支付 API
        /// </summary>
        public string paySign { get; set; }
    }
}
