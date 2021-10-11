using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付参数
    /// </summary>
    public partial class PayParameter : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public PayParameter() { }

        public PayParameter(Guid id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
        /// <summary>
        /// 支付参数值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentType? PaymentType { get; set; }
        /// <summary>
        /// 收单机构
        /// </summary>
        public PayOrganizationType PayOrganizationType { get; set; }
    }
}