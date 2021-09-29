using System;
using Adee.Store.Pays;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Domain.Tenants
{
    /// <summary>
    /// 租户扩展信息
    /// </summary>
    public class TenantExt : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 软件编号
        /// </summary>
        public string SoftwareCode { get; set; }

        /// <summary>
        /// 支付参数版本
        /// </summary>
        public long? PaypameterVersion { get; set; }
    }
}
