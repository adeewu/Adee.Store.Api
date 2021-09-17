using System;

namespace Adee.Store.Pays
{
    public class QueryPayStatusArgs
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 使用支付参数版本
        /// </summary>
        public long PayParameterVersion { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid TenantId { get; set; }

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
