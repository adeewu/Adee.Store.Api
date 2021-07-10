using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// 扫码支付
        /// </summary>
        [Description("扫码支付")]
        QRPay = 1,

        /// <summary>
        /// 微信支付
        /// </summary>
        [Description("微信支付")]
        WechatPay = 2,

        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay = 3,

        /// <summary>
        /// 龙支付
        /// </summary>
        [Description("龙支付")]
        CCB = 4,

        /// <summary>
        /// 银联支付
        /// </summary>
        [Description("银联支付")]
        UnionPay = 5,
    }
}
