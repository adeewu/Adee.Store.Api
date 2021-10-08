using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付订单
    /// </summary>
    public partial class PayOrder : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public PayOrder() { }

        public PayOrder(Guid id) : this()
        {
            Id = id;
            PayRefunds = new List<PayRefund>();
            PayOrderLogs = new List<PayOrderLog>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 收款金额，单位：分
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string NotifyUrl { get; set; }
        /// <summary>
        /// 收款标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 业务订单号
        /// </summary>
        public string BusinessOrderId { get; set; }
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }
        /// <summary>
        /// 收单机构订单号
        /// </summary>
        public string PayOrganizationOrderId { get; set; }
        /// <summary>
        /// 支付参数版本
        /// </summary>
        public long ParameterVersion { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentType PaymentType { get; set; }
        /// <summary>
        /// 收单机构
        /// </summary>
        public PayOrganizationType PayOrganizationType { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymethodType PaymethodType { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PayTaskStatus Status { get; set; }
        /// <summary>
        /// 支付状态描述
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 查询状态
        /// </summary>
        public PayTaskStatus? QueryStatus { get; set; }
        /// <summary>
        /// 查询状态描述
        /// </summary>
        public string QueryStatusMessage { get; set; }
        /// <summary>
        /// 通知状态
        /// </summary>
        public PayTaskStatus? NotifyStatus { get; set; }
        /// <summary>
        /// 通知状态描述
        /// </summary>
        public string NotifyStatusMessage { get; set; }
        /// <summary>
        /// 取消状态
        /// </summary>
        public PayTaskStatus? CancelStatus { get; set; }
        /// <summary>
        /// 取消状态描述
        /// </summary>
        public string CancelStatusMessage { get; set; }
        /// <summary>
        /// 退款状态
        /// </summary>
        public PayTaskStatus? RefundStatus { get; set; }
        /// <summary>
        /// 退款状态描述
        /// </summary>
        public string RefundStatusMessage { get; set; }        
        /// <summary>
        /// 成功退款次数
        /// </summary>
        public int? RefundCount { get; set; }
        /// <summary>
        /// 支付备注
        /// </summary>
        public string PayRemark { get; set; }

        /// <summary>
        /// 退款记录
        /// </summary>
        public virtual ICollection<PayRefund> PayRefunds { get; set; }

        /// <summary>
        /// 订单记录
        /// </summary>
        public virtual ICollection<PayOrderLog> PayOrderLogs { get; set; }
    }
}