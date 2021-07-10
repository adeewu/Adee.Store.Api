using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessType
    {
        /// <summary>
        /// 未分类
        /// </summary>
        [Description("未分类")]
        Unknown = 0,

        /// <summary>
        /// 网页收银台
        /// </summary>
        [Description("网页收银台")]
        WebCheckout = 1,

        /// <summary>
        /// 安卓收银台
        /// </summary>
        [Description("安卓收银台")]
        AndroidCheckout = 2,

        /// <summary>
        /// IOS收银台
        /// </summary>
        [Description("IOS收银台")]
        IOSCheckout = 3,

        /// <summary>
        /// 在线商城
        /// </summary>
        [Description("在线商城")]
        OnlineShop = 4,

        /// <summary>
        /// 扫码点餐
        /// </summary>
        [Description("扫码点餐")]
        ScanBooking = 5,

        /// <summary>
        /// 无码收银
        /// </summary>
        [Description("无码收银")]
        NoCodePay = 6,

        /// <summary>
        /// 会员充值
        /// </summary>
        [Description("会员充值")]
        MemberRecharge = 7,

        /// <summary>
        /// 会员充次
        /// </summary>
        [Description("会员充次")]
        MemberRechargeCount = 8,

        /// <summary>
        /// 小程序订单
        /// </summary>
        [Description("小程序订单")]
        MiniProOrder = 9,
    }
}
