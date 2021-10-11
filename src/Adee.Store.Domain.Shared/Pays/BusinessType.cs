using System.ComponentModel;

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
        /// 无码收银
        /// </summary>
        [Description("无码收银")]
        NoCodePay = 1,

        /// <summary>
        /// 网页收银台
        /// </summary>
        [Description("网页收银台")]
        WebCheckout = 2,
    }
}
