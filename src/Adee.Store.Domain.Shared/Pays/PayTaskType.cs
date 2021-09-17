using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum PayTaskType
    {
        /// <summary>
        /// Unknow
        /// </summary>
        [Description("Unknow")]
        Unknow = 0,

        /// <summary>
        /// B2C
        /// </summary>
        [Description("B2C")]
        B2C,

        /// <summary>
        /// C2B
        /// </summary>
        [Description("C2B")]
        C2B,

        /// <summary>
        /// Query
        /// </summary>
        [Description("Query")]
        Query,

        /// <summary>
        /// Refund
        /// </summary>
        [Description("Refund")]
        Refund,

        /// <summary>
        /// WechatJSPay
        /// </summary>
        [Description("WechatJS")]
        WechatJSPay,

        /// <summary>
        /// AssertNotify
        /// </summary>
        [Description("AssertNotify")]
        AssertNotify,

        /// <summary>
        /// Cancel
        /// </summary>
        [Description("Cancel")]
        Cancel,
    }
}
