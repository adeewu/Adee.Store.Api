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
        B2C = 1,

        /// <summary>
        /// C2B
        /// </summary>
        [Description("C2B")]
        C2B = 2,

        /// <summary>
        /// Query
        /// </summary>
        [Description("Query")]
        Query = 3,

        /// <summary>
        /// Refund
        /// </summary>
        [Description("Refund")]
        Refund = 4,

        /// <summary>
        /// RefundQuery
        /// </summary>
        [Description("RefundQuery")]
        RefundQuery = 5,

        /// <summary>
        /// WechatJSPay
        /// </summary>
        [Description("WechatJS")]
        WechatJSPay = 6,

        /// <summary>
        /// AssertNotify
        /// </summary>
        [Description("AssertNotify")]
        AssertNotify = 7,

        /// <summary>
        /// Cancel
        /// </summary>
        [Description("Cancel")]
        Cancel = 8,
    }
}
