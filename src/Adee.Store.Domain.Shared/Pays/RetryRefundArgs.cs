using System;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 重试退款
    /// </summary>
    public class RetryRefundArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 执行间隔
        /// </summary>
        public int[] Rates { get; set; }
    }
}
