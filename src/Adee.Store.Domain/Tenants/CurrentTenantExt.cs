using System;
using System.Threading.Tasks;
using Adee.Store.Domain.Shared.Tenants;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Domain.Tenants
{
    public class CurrentTenantExt : ICurrentTenantExt, ITransientDependency
    {
        private readonly IDistributedCache<TenantCacheItem> _cache;
        private readonly ICurrentTenant _currentTenant;

        public CurrentTenantExt(IDistributedCache<TenantCacheItem> cache, ICurrentTenant currentTenant)
        {
            _cache = cache;
            _currentTenant = currentTenant;
        }

        public async Task<string> GetSoftwareCodeAsync()
        {
            return await GetItem().ContinueWith(p => p.Result.GetPropValue(c => c.SoftwareCode));
        }

        private async Task<TenantCacheItem> GetItem()
        {
            return await _cache.GetAsync(_currentTenant.Id.Value.ToString());
        }
    }
}
