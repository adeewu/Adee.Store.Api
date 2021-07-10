using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public class ProductCatalog : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public ProductCatalog(Guid id)
        {
            Id = id;
        }

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
    }
}
