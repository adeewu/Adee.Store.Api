using System.ComponentModel;

namespace Adee.Store.Products
{
    /// <summary>
    /// 售卖类型
    /// </summary>
    public enum ProductSaleType
    {
        /// <summary>
        /// 单品
        /// </summary>
        [Description("商品")]
        Product = 1,

        /// <summary>
        /// 无码商品
        /// </summary>
        [Description("无码商品")]
        NoneProduct = 2,

        /// <summary>
        /// 套餐
        /// </summary>
        [Description("套餐")]
        Package = 3
    }
}
