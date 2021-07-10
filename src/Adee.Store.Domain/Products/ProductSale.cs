using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品售卖
    /// </summary>
    public partial class ProductSale : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public ProductSale(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 售卖标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 推荐售卖
        /// </summary>
        public bool Recommend { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 售卖单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 售卖量
        /// </summary>
        public decimal SaleVolume { get; set; }

        /// <summary>
        /// 总售卖量
        /// </summary>
        public decimal TotalSaleVolume { get; set; }

        /// <summary>
        /// 允许超售
        /// </summary>
        public bool AllowOversell { get; set; }

        /// <summary>
        /// 折扣，百分比
        /// </summary>
        public int? Discount { get; set; }

        /// <summary>
        /// 状态，1：正常，-1：下架
        /// </summary>
        public ProductSaleStatus Status { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 售卖类型
        /// </summary>
        public ProductSaleType ProductSaleType { get; set; }


        /// <summary>
        /// 售卖详情
        /// </summary>
        public virtual ICollection<ProductSaleInfo> ProductSaleInfos { get; set; }
    }
}
