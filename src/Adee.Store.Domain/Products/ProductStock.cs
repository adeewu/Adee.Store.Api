using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public partial class ProductStock : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public ProductStock(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 质保期
        /// </summary>
        public decimal? Warranty { get; set; }

        /// <summary>
        /// 质保期单位
        /// </summary>
        public string WarrantyUnit { get; set; }


        /// <summary>
        /// 所属商品
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// 入库记录
        /// </summary>
        public virtual ICollection<ProductStockLog> ProductStockLogs { get; set; }

        /// <summary>
        /// 相关售卖
        /// </summary>
        public virtual ICollection<ProductSaleInfo> ProductSaleDetails { get; set; }
    }
}
