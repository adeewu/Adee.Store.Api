using System.ComponentModel;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PaymethodType
    {
        /// <summary>
        /// 未分类
        /// </summary>
        [Description("未分类")]
        Unknown = 0,

        /// <summary>
        /// B2C
        /// </summary>
        [Description("B2C")]
        B2C = 1,

        /// <summary>
        /// C2B
        /// </summary>
        [Description("C2B")]
        C2B = 2,

        /// <summary>
        /// 微信小程序
        /// </summary>
        [Description("WechatMiniPro")]
        WechatMiniPro = 3,

        /// <summary>
        /// 微信公众号
        /// </summary>
        [Description("WechatMP")]
        WechatMP = 4,

        /// <summary>
        /// 支付宝小程序
        /// </summary>
        [Description("AlipayMiniPro")]
        AlipayMiniPro = 5,

        /// <summary>
        /// 支付宝服务窗
        /// </summary>
        [Description("AlipayMP")]
        AlipayMP = 6,

        /// <summary>
        /// 银联JS支付
        /// </summary>
        [Description("UnionJSPay")]
        UnionJSPay = 7,
    }
}
