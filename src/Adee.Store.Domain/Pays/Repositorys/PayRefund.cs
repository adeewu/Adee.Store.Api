using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付订单退款记录
    /// </summary>
    public partial class PayRefund : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public PayRefund() { }

        public PayRefund(Guid id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 支付订单Id
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundOrderId { get; set; }
        /// <summary>
        /// 退款金额，单位：分
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 退款状态
        /// </summary>
        public PayTaskStatus Status { get; set; }
        /// <summary>
        /// 退款状态描述
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 查询退款状态
        /// </summary>
        public PayTaskStatus? QueryStatus { get; set; }
        /// <summary>
        /// 查询退款状态描述
        /// </summary>
        public string QueryStatusMessage { get; set; }
        /// <summary>
        /// 退款通知状态
        /// </summary>
        public PayTaskStatus? NotifyStatus { get; set; }
        /// <summary>
        /// 退款通知状态描述
        /// </summary>
        public string NotifyStatusMessage { get; set; }

        public virtual PayOrder PayOrder { get; set; }
    }
}