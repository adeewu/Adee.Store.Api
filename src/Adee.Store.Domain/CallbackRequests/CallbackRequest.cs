using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.CallbackRequests
{
    /// <summary>
    /// 回调请求
    /// </summary>
    public class CallbackRequest : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 回调类型
        /// </summary>
        public CallbackType CallbackType { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 请求正文
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Method、Url、Body、Query、Header经过MD5计算的值
        /// </summary>
        public string HashCode { get; set; }
    }
}
