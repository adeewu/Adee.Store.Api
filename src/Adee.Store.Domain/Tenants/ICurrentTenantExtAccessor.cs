using System.Threading.Tasks;

namespace Adee.Store.Domain.Tenants
{
    public interface ICurrentTenantExtAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        TenantCacheItem Current { get; set; }

        /// <summary>
        /// 设置参数版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Task SetParameterVersion(long version);
    }
}
