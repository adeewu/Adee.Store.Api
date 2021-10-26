using System.ComponentModel;

namespace Adee.Store.CallbackRequests
{
    /// <summary>
    /// 
    /// </summary>
    public enum CallbackType
    {
        /// <summary>
        /// 微信第三方平台授权事件
        /// </summary>
        [Description("微信第三方平台授权事件")]
        WechatComponentAuthNoity = 1,

        /// <summary>
        /// 微信第三方平台消息与事件通知
        /// </summary>
        [Description("微信第三方平台授权事件")]
        WechatComponentNotify = 2,

        /// <summary>
        /// 支付回调通知
        /// </summary>
        [Description("支付回调通知")]
        PayNotify = 3,
    }
}
