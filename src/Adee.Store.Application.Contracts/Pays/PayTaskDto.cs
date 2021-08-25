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

        /// <summary>
        /// C2B
        /// </summary>
        [Description("C2B")]
        C2B
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

    /// <summary>
    /// 支付任务
    /// </summary>
    public class PayTaskDto
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        /// <value></value>
        [Required]
        public TaskType TaskType { get; set; }
    }

    /// <summary>
    /// B2C收款任务
    /// </summary>
    public class B2CPayTaskDto : PayTaskDto
    {
        public B2CPayTaskDto()
        {
            TaskType = TaskType.B2C;
        }

        /// <summary>
        /// 支付码
        /// </summary>
        [Required]
        public string AuthCode { get; set; }
    }
}
