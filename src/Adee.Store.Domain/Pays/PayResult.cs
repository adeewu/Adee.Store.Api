using System;
using System.Collections.Generic;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付结果
    /// </summary>
    public class PayTaskResult
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        public PayTaskStatus Status { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    /// 支付任务结果
    /// </summary>
    public class PayTaskSuccessResult : PayTaskResult
    {
        /// <summary>
        /// 收款时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 收款金额，单位：分
        /// </summary>
        public int? Money { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentType? PaymentType { get; set; }

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
    }

    /// <summary>
    /// 退款结果
    /// </summary>
    public class PayTaskRefundResult : PayTaskResult
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundOrderId { get; set; }
    }
}
