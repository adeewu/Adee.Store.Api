using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Adee.Store.Products
{
    public partial class ProductStockLog : AuditedAggregateRoot<Guid>
    {
        public ProductStockLog(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 库存Id
        /// </summary>
        public Guid ProductStockId { get; set; }

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
        /// 库存来源
        /// </summary>
        public string Source { get; set; }


        /// <summary>
        /// 所属商品库存
        /// </summary>
        public virtual ProductStock ProductStock { get; set; }
    }
}
