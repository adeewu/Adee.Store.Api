using System.Threading.Tasks;

namespace Adee.Store.Domain.Tenants
{
    /// <summary>
    /// 租户扩展信息
    /// </summary>
    public interface ICurrentTenantExt
    {
        /// <summary>
        /// 软件编号
        /// </summary>
        string SoftwareCode { get; }

        /// <summary>
        /// 支付参数版本
        /// </summary>
        long? PaypameterVersion { get; }

        /// <summary>
        /// 设置支付参数版本
        /// </summary>
        /// <param name="version"></param>
        Task SetPaypameterVersion(long version);
    }
}
