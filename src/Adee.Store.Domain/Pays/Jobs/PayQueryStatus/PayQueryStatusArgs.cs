using System;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    public class PayQueryStatusArgs : LoopJobArgs, IMultiTenant
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
