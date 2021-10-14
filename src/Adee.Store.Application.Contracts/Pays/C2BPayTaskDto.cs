namespace Adee.Store.Pays
{
    /// <summary>
    /// JSApi收款任务
    /// </summary>
    public class C2BPayTaskDto : BasePayTaskDto
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 支付超时时间，单位：分钟，默认5分钟
        /// </summary>
        public int PayExpire { get; set; } = PayConsts.OrderTimeExpire;
    }
}
