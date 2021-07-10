using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Adee.Store.Orders
{
    /// <summary>
    /// 执行状态
    /// </summary>
    [Description("执行状态")]
    public enum ExcutingStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = -1,

        /// <summary>
        /// 待执行
        /// </summary>
        [Description("待执行")]
        Normal = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,

        /// <summary>
        /// 执行中
        /// </summary>
        [Description("执行中")]
        Excuting = 2
    }
}
