using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public class ProductDto : EntityDto<Guid>, IMultiTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid CatalogId { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public string Catalog { get; set; }

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
        /// 库存，冗余值
        /// </summary>
        public decimal Stock { get; set; }

        /// <summary>
        /// 销量，冗余值
        /// </summary>
        public decimal SaleVolume { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public List<ProductSpec> Specs { get; set; } = new List<ProductSpec>();
    }

    public class CreateUpdateProductDto
    {
        /// <summary>
        /// 产品分类
        /// </summary>
        [Required]
        public Guid CatalogId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string BarCode { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 计价方式
        /// </summary>
        [Required]
        public PricingType PricingType { get; set; }

        /// <summary>
        /// 助记码
        /// </summary>
        [StringLength(50)]
        public string QuickCode { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [StringLength(50)]
        public string ProductBrand { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [StringLength(50)]
        public string UnitName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public List<ProductSpec> Specs { get; set; } = new List<ProductSpec>();
    }

    public class ProductListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 商品名称，模糊查找
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        public Guid? CatalogId { get; set; }

        /// <summary>
        /// 商品码
        /// </summary>
        public string BarCode { get; set; }
    }
}
