using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付订单记录
    /// </summary>
    public partial class PayOrderLog : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public PayOrderLog() { }

        public PayOrderLog(Guid id) : this()
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
        /// 记录类型
        /// </summary>
        public OrderLogType LogType { get; set; }
        /// <summary>
        /// 记录状态
        /// </summary>
        public PayTaskStatus Status { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 异常描述
        /// </summary>
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// 原始提交报文
        /// </summary>
        public string OriginRequest { get; set; }
        /// <summary>
        /// 提交报文
        /// </summary>
        public string SubmitRequest { get; set; }
        /// <summary>
        /// 原始响应报文
        /// </summary>
        public string OriginResponse { get; set; }
        /// <summary>
        /// 解密响应报文
        /// </summary>
        public string EncryptResponse { get; set; }

        public virtual PayOrder PayOrder { get; set; }
    }
}