using System;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 重试通知
    /// </summary>
    public class PayNotifyArgs : IMultiTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 执行间隔
        /// </summary>
        public int[] Rates { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 退款通知
        /// </summary>
        public bool IsRefundNotify { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public object NotifyContent { get; set; }
    }
}
