using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public class ProductStockLogDto : EntityDto<Guid>, IMultiTenant, IHasCreationTime
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime CreationTime { get; set; }

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
    }

    public class ProductStockLogListDto : PagedAndSortedResultRequestDto
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
        /// 入库开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 入库结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }

    public class CreateUpdateProductStockLogDto
    {

    }
}
