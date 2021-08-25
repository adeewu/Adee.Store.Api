using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
