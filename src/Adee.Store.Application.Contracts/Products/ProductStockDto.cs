using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public class ProductStockDto : EntityDto<Guid>, IMultiTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 进货价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 进货量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 库存批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 原产地
        /// </summary>
        public string OriginPlace { get; set; }

        /// <summary>
        /// 质保期
        /// </summary>
        public decimal? Warranty { get; set; }

        /// <summary>
        /// 质保期单位
        /// </summary>
        public string WarrantyUnit { get; set; }

        /// <summary>
        /// 库存来源
        /// </summary>
        public string Source { get; set; }
    }

    public class ProductStockListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Filter { get; set; }
    }

    public class CreateUpdateProductStockDto
    {
        /// <summary>
        /// 质保期
        /// </summary>
        public string Warranty { get; set; }

        /// <summary>
        /// 质保期单位
        /// </summary>
        public string warrantyUnit { get; set; }
    }
}
