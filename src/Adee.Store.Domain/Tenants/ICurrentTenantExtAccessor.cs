using System;
using System.Threading.Tasks;
using Adee.Store.Pays;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

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
