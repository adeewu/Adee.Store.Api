using System;
using System.Collections.Generic;
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
        /// 请求方式
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求正文
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// Method、Url、Body、Query经过MD5计算的值
        /// </summary>
        public string HashCode { get; set; }
        /// <summary>
        /// 通知执行状态
        /// </summary>
        public PayTaskStatus Status { get; set; }
        /// <summary>
        /// 通知状态状态描述
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 支付结果查询Id
        /// </summary>
        public string MerchantOrderId { get; set; }
        /// <summary>
        /// 支付订单Id
        /// </summary>
        public string PayOrderId { get; set; }
    }
}