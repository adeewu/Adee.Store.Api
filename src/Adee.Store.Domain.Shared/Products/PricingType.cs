using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Adee.Store.Products
{
    /// <summary>
    /// 计价类型
    /// </summary>
    [Description("计价类型")]
    public enum PricingType
    {
        /// <summary>
        /// 计件
        /// </summary>
        [Description("计件")]
        Piece = 1,

        /// <summary>
        /// 计重
        /// </summary>
        [Description("计重")]
        Weight = 2,
    }
}
