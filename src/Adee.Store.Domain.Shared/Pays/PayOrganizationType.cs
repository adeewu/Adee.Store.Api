using System.ComponentModel;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 收单机构
    /// </summary>
    public enum PayOrganizationType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,

        /// <summary>
        /// 支付宝、微信官方
        /// </summary>
        [Description("官方")]
        Official = 1,

        /// <summary>
        /// 天阙
        /// </summary>
        [Description("天阙")]
        TianQue = 2,
    }
}
