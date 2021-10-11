using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public class ProductStockOrderDto : EntityDto<Guid>, IMultiTenant, IHasCreationTime
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 库存批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal ActualMoney { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string Payment { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class ProductStockOrderListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Filter { get; set; }
    }

    public class CreateUpdateProductStockOrderDto
    {
        /// <summary>
        /// 库存批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal ActualMoney { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string Payment { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public List<ProductStockOrderItem> Products { get; set; }
    }

    /// <summary>
    /// 入库商品
    /// </summary>
    public class ProductStockOrderItem
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

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
        /// 原产地
        /// </summary>
        public string OriginPlace { get; set; }
    }
}
