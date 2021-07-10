using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品售卖详情
    /// </summary>
    public class ProductSaleInfo : Entity<Guid>
    {
        public ProductSaleInfo(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 库存Id
        /// </summary>
        public Guid ProductStockId { get; set; }

        /// <summary>
        /// 商品售卖Id
        /// </summary>
        public Guid ProductSaleId { get; set; }

        /// <summary>
        /// 售卖量
        /// </summary>
        public decimal Quantity { get; set; }

        public virtual ProductStock ProductStock { get; set; }

        public virtual ProductSale ProductSale { get; set; }
    }
}
