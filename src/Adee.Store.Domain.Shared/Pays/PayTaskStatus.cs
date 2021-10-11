using System.ComponentModel;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum PayTaskStatus
    {
        /// <summary>
        /// 执行失败
        /// </summary>
        [Description("执行失败")]
        Faild = -1,

        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        Normal = 0,

        /// <summary>
        /// 执行成功
        /// </summary>
        [Description("执行成功")]
        Success = 1,

        /// <summary>
        /// 待执行
        /// </summary>
        [Description("待执行")]
        Waiting = 2,

        /// <summary>
        /// 执行中
        /// </summary>
        [Description("执行中")]
        Executing = 3,
    }
}
