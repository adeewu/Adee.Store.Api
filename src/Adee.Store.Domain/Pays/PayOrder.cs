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
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 支付结果查询Id
        /// </summary>
        public string MerchantOrderId { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 支付Id
        /// </summary>
        public string PayOrderId { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PayTaskStatus Status { get; set; }
        /// <summary>
        /// 支付参数版本
        /// </summary>
        public int ParameterVersion { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentType PaymentType { get; set; }
        /// <summary>
        /// 收单机构
        /// </summary>
        public PayOrganizationType PayOrganizationType { get; set; }
        /// <summary>
        /// 支付状态描述
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime? OrderTime { get; set; }
        /// <summary>
        /// 查询状态
        /// </summary>
        public PayTaskStatus QueryStatus { get; set; }
        /// <summary>
        /// 通知状态
        /// </summary>
        public PayTaskStatus NotifyStatus { get; set; }
        /// <summary>
        /// 查询状态描述
        /// </summary>
        public string QueryStatusMessage { get; set; }
        /// <summary>
        /// 通知状态描述
        /// </summary>
        public string NotifyStatusMessage { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymethodType PaymethodType { get; set; }
        /// <summary>
        /// 订单数据
        /// </summary>
        public string OrderData { get; set; }
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
        /// 通知地址
        /// </summary>
        public string NotifyUrl { get; set; }
        /// <summary>
        /// 发起支付域名
        /// </summary>
        public string TargetDomain { get; set; }
        /// <summary>
        /// 业务模块类型
        /// </summary>
        public BusinessType BusinessType { get; set; }
        /// <summary>
        /// 收款标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 收单机构订单号
        /// </summary>
        public string PayOrganizationOrderId { get; set; }

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