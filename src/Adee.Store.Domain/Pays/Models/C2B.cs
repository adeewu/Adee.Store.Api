namespace Adee.Store.Pays
{
    /// <summary>
    /// JS支付
    /// </summary>
    public class C2B : BasePayTask
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 支付超时时间，单位：分钟
        /// </summary>
        public int PayExpire { get; set; }
    }
}
