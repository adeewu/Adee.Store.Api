using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public BusinessType businessType { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string NotifyUrl { get; set; }
    }

    /// <summary>
    /// 收款任务结果
    /// </summary>
    public class PayStatusResultDto
    {
        /// <summary>
        /// 业务订单号
        /// </summary>
        public string BusinessOrderId { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public PayTaskStatus Status { get; set; }

        /// <summary>
        /// 收款状态描述
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// B2C收款任务
    /// </summary>
    public class RefundPayTaskDto
    {
        /// <summary>
        /// 收款金额，单位：分
        /// </summary>
        [Required]
        public int Money { get; set; }

        /// <summary>
        /// 订单号，支持BusinessOrderId、PayOrderId、PayOrganizationOrderId
        /// </summary>
        [Required]
        public string OrderId { get; set; }
    }
}
