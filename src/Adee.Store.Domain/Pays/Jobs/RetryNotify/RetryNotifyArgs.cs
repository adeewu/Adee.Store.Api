using System;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 重试通知
    /// </summary>
    public class RetryNotifyArgs : PayTaskSuccessResult
    {
        /// <summary>
        /// 
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

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string NoitfyUrl { get; set; }
    }
}
