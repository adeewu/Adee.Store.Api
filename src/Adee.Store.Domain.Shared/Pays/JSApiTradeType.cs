using System.ComponentModel;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 
    /// </summary>
    public enum JSApiTradeType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// 微信公众号/支付宝服务窗/银联js支付/支付宝小程序
        /// </summary>
        [Description("MP")]
        MP = 1,

        /// <summary>
        /// 微信小程序
        /// </summary>
        [Description("MINIPRO")]
        MINIPRO = 2,
    }
}
