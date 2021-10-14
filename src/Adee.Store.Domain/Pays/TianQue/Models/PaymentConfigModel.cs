namespace Adee.Store.Domain.Pays.TianQue.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentConfigModel
    {
        /// <summary>
        /// 商家Id
        /// </summary>
        public string mno { get; set; }

        /// <summary>
        /// 服务商Id
        /// </summary>
        public string orgId { get; set; }

        /// <summary>
        /// 服务商公钥
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// 服务商私钥
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// 服务商appId
        /// </summary>
        public string ServiceAppId { get; set; }
    }
}
