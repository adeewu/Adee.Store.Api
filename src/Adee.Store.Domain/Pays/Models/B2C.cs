namespace Adee.Store.Pays
{
    /// <summary>
    /// 扫码支付
    /// </summary>
    public class B2C : BasePayTask
    {
        /// <summary>
        /// 收款码
        /// </summary>
        public string AuthCode { get; set; }
    }
}
