using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// B2C
        /// </summary>
        [Description("B2C")]
        B2C,
    }

    /// <summary>
    /// 通用支付任务内容
    /// </summary>
    public class PayTaskContentDto
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskType TaskType { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string MerchantOrderId { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        public string Content { get; set; }
    }

    public class PayTaskDto
    {
        [Required]
        public TaskType TaskType { get; set; }
    }

    public class B2CPayTaskDto : PayTaskDto
    {
        /// <summary>
        /// 支付码
        /// </summary>
        public string AuthCode { get; set; }
    }
}
