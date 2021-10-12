using System;
using System.Collections.Generic;
using System.Text;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 
    /// </summary>
    public class PayConsts
    {
        /// <summary>
        /// 查询时长，秒
        /// </summary>
        public static int QueryDuration { get; set; } = 600;

        /// <summary>
        /// 退款查询时长，秒
        /// </summary>
        public static int RefundQueryDuration { get; set; } = 600;
    }
}
