using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public partial class Product : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Product()
        {
            ProductStocks = new HashSet<ProductStock>();
        }

        public Product(Guid id) : this()
        {
            Id = id;
        }


        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid CatalogId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 计价方式，1：计件，2：计重
        /// </summary>
        public PricingType PricingType { get; set; }

        /// <summary>
        /// 助记码
        /// </summary>
        public string QuickCode { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string ProductBrand { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 商品规格，单规格商品留空
        /// </summary>
        public string Specs { get; set; }

        /// <summary>
        /// 库存，冗余值
        /// </summary>
        public decimal Stock { get; set; }

        /// <summary>
        /// 销量，冗余值
        /// </summary>
        public decimal SaleVolume { get; set; }


        /// <summary>
        /// 商品分类
        /// </summary>
        public virtual ProductCatalog ProductCatalog { get; set; }

        /// <summary>
        /// 库存记录
        /// </summary>
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
    }
}
