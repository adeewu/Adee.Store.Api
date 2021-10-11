using System.ComponentModel;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 付款方式
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// 未分类
        /// </summary>
        [Description("未分类")]
        Unknown = 0,

        /// <summary>
        /// 微信支付
        /// </summary>
        [Description("微信支付")]
        WechatPay = 1,

        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay = 2,

        /// <summary>
        /// 云闪付
        /// </summary>
        [Description("云闪付")]
        UnionPay = 3,

        /// <summary>
        /// 龙支付
        /// </summary>
        [Description("龙支付")]
        CCB = 4,
    }
}
