using Adee.Store.Pays;
using Adee.Store.Products;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Orders
{
    /// <summary>
    /// 订单
    /// </summary>
    public partial class Order : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Order()
        {
            OrderInfos = new HashSet<OrderInfo>();
        }

        public Order(Guid id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 订单流水号
        /// </summary>
        public string RunningId { get; set; }

        /// <summary>
        /// 支付业务号
        /// </summary>
        public string MerchantOrderId { get; set; }

        /// <summary>
        /// 订单标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public TerminalType TerminalType { get; set; }

        /// <summary>
        /// 销售单价，单一商品有效
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public string Payment { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public ExcutingStatus PayStatus { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public ExcutingStatus OrderStatus { get; set; }


        /// <summary>
        /// 订单详情
        /// </summary>
        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
    }
}
