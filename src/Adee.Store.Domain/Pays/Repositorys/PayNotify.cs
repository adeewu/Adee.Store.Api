using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付回调通知
    /// </summary>
    public partial class PayNotify : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public PayNotify() { }

        public PayNotify(Guid id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 回调Id
        /// </summary>
        public Guid CallbackRequestId { get; set; }
        /// <summary>
        /// 通知执行状态
        /// </summary>
        public PayTaskStatus Status { get; set; }
        /// <summary>
        /// 通知状态状态描述
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 业务订单号
        /// </summary>
        public string BusinessOrderId { get; set; }
        /// <summary>
        /// 收单机构订单号
        /// </summary>
        public string PayOrganizationOrderId { get; set; }
        /// <summary>
        /// 支付订单Id
        /// </summary>
        public string PayOrderId { get; set; }
    }
}