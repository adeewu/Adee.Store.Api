using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Products
{
    public partial class ProductStockOrder : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public ProductStockOrder()
        {

        }

        public ProductStockOrder(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

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
}
