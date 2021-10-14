namespace Adee.Store.Domain.Pays.TianQue.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentConfigModel
    {
        /// <summary>
        /// �̼�Id
        /// </summary>
        public string mno { get; set; }

        /// <summary>
        /// ������Id
        /// </summary>
        public string orgId { get; set; }

        /// <summary>
        /// �����̹�Կ
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// ������˽Կ
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// ������appId
        /// </summary>
        public string ServiceAppId { get; set; }
    }
}
