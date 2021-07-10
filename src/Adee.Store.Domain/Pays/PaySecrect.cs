using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    public partial class PaySecrect : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public PaySecrect() { }

        public PaySecrect(Guid id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string SignKey { get; set; }
    }
}