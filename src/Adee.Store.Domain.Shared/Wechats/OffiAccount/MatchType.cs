using System.ComponentModel;

namespace Adee.Store.Wechats.OffiAccount
{
    /// <summary>
    /// 匹配类型
    /// </summary>
    public enum MatchType
    {
        /// <summary>
        /// 租户默认
        /// </summary>
        [Description("租户默认")]
        Default = 1,

        /// <summary>
        /// 全匹配
        /// </summary>
        [Description("全匹配")]
        Full = 2,

        /// <summary>
        /// 开头匹配
        /// </summary>
        [Description("开头匹配")]
        StartLike = 3,

        /// <summary>
        /// 结尾匹配
        /// </summary>
        [Description("结尾匹配")]
        EndLike = 4,

        /// <summary>
        /// 模糊匹配
        /// </summary>
        [Description("模糊匹配")]
        Like = 5,
    }
}
