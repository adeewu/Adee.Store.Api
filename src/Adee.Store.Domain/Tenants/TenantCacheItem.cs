namespace Adee.Store.Domain.Tenants
{
    /// <summary>
    /// 租户缓存数据
    /// </summary>
    public class TenantCacheItem
    {
        /// <summary>
        /// 软件编号
        /// </summary>
        public string SoftwareCode { get; set; }

        /// <summary>
        /// 支付参数版本
        /// </summary>
        public long? PaypameterVersion { get; set; }
    }
}
