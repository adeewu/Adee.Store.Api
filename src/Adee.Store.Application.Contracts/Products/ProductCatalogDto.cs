using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductCatalogDto : EntityDto<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类路径，计算值
        /// </summary>
        public string CatalogPath { get; set; }

        /// <summary>
        /// 父分类Id
        /// </summary>
        public Guid? ParentCatalogId { get; set; }

        /// <summary>
        /// 子分类集合
        /// </summary>
        public List<ProductCatalogDto> SubProductCatalogs { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateUpdateProductCatalogDto
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 父分类Id
        /// </summary>
        public Guid? ParentCatalogId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProductCatalogListDto : PagedAndSortedResultRequestDto
    {

    }
}
