using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Unknown = -1,

        /// <summary>
        /// 新大陆
        /// </summary>
        [Description("新大陆")]
        StartPosPay = 1,

        /// <summary>
        /// CCBPay
        /// </summary>
        [Description("CCBPay")]
        CCBPay = 2,

        /// <summary>
        /// 龙支付定时上单
        /// </summary>
        [Description("龙支付定时上单")]
        CCBPaySync = 3,
    }
}
