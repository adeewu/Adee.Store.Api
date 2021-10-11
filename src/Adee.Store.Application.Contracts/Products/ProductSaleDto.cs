using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public class ProductSaleDto : EntityDto<Guid>, IMultiTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }
    }

    public class ProductSaleListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }

    public class CreateUpdateProductSaleDto
    {

    }
}
