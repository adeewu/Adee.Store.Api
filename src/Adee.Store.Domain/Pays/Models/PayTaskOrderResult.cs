using System;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付任务结果
    /// </summary>
    public class PayTaskOrderResult : PayTaskResult
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
}
