using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Adee.Store.Products
{
    /// <summary>
    /// 
    /// </summary>
    [Description("销售状态")]
    public enum ProductSaleStatus
    {
        /// <summary>
        /// 下架
        /// </summary>
        [Description("下架")]
        SellOff = -1,

        /// <summary>
        /// 暂存
        /// </summary>
        [Description("暂存")]
        Normal = 0,

        /// <summary>
        /// 售卖中
        /// </summary>
        [Description("售卖中")]
        Selling = 1,
    }
}
