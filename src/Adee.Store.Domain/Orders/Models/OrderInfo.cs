using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Orders
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public partial class OrderInfo : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public OrderInfo(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 数据Id
        /// </summary>
        public Guid? DataId { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        public string DataName { get; set; }

        /// <summary>
        /// 成交单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 成交总价
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 成交描述
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// 所属订单
        /// </summary>
        public virtual Order Order { get; set; }
    }
}
