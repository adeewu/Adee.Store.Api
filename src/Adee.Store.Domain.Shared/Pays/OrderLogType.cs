﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付订单记录类型
    /// </summary>
    public enum OrderLogType
    {
        /// <summary>
        /// 支付
        /// </summary>
        [Description("支付")]
        Pay = 1,

        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        Query = 2,

        /// <summary>
        /// 通知
        /// </summary>
        [Description("通知")]
        Notify = 3,

        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refund = 4,
    }
}
