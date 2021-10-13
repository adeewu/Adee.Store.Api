using System.ComponentModel.DataAnnotations;

namespace Adee.Store.Pays
{
    /// <summary>
    /// B2C收款任务
    /// </summary>
    public class B2CPayTaskDto : BasePayTaskDto
    {
        /// <summary>
        /// 收款码
        /// </summary>
        [Required]
        public string AuthCode { get; set; }
    }
}
