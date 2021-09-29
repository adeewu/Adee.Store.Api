using System;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 查询订单缓存项
    /// </summary>
    public class QueryOrderCacheItem
    {
        /// <summary>
        /// 收款时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal? Money { get; set; }

        /// <summary>
        /// 收单机构订单号
        /// </summary>
        public string PayOrganizationOrderId { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }

        /// <summary>
        /// 业务订单号
        /// </summary>
        public string BusinessOrderId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public PayTaskStatus Status { get; set; }

        /// <summary>
        /// 任务消息
        /// </summary>
        public string Message { get; set; }
    }
}