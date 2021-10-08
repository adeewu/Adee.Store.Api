using System.ComponentModel.DataAnnotations;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 原路退回
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
