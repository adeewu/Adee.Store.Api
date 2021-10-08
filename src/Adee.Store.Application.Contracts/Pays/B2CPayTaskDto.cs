using System.ComponentModel.DataAnnotations;

namespace Adee.Store.Pays
{
    /// <summary>
    /// B2C收款任务
    /// </summary>
    public class B2CPayTaskDto
    {
        /// <summary>
        /// 收款码
        /// </summary>
        [Required]
        public string AuthCode { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        [Required]
        public int Money { get; set; }

        /// <summary>
        /// 收款标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 收款备注
        /// </summary>
        [Required]
        public string PayRemark { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        [Required]
        public string IPAddress { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [Required]
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        [Required]
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 业务订单号
        /// </summary>
        [Required]
        public string BusinessOrderId { get; set; }
    }
}
