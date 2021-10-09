using System;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 查询退款状态
    /// </summary>
    public class RefundQueryStatusArgs : LoopJobArgs, IMultiTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundPayOrderId { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public int Money { get; set; }
    }
}
