using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 
    /// </summary>
    public class PayParameterDto : EntityDto<Guid>, IMultiTenant
    {
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

    public class CreateUpdatePayParameterDto
    {
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

    public class PayParameterListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
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
