using System.ComponentModel;

namespace Adee.Store.Orders
{
    /// <summary>
    /// 订单类型
    /// </summary>
    [Description("订单类型")]
    public enum OrderType
    {
        /// <summary>
        /// 商品
        /// </summary>
        [Description("商品")]
        Product = 1,

        /// <summary>
        /// 商品套餐
        /// </summary>
        [Description("商品套餐")]
        ProductPackage = 2,
    }
}
