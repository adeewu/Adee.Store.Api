using System;
using System.Threading;
using System.Threading.Tasks;
using Adee.Store.Pays;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Domain.Tenants
{
    public class CurrentTenantExtAccessor : ICurrentTenantExtAccessor, ITransientDependency
    {
        private readonly IDistributedCache<TenantCacheItem> _cache;
        private readonly ICurrentTenant _currentTenant;
        private readonly IRepository<TenantExt> _tenantExtRepository;
        private readonly IRepository<PayParameter> _payParameterRepository;
        private readonly AsyncLocal<TenantCacheItem> _currentScope;

        public CurrentTenantExtAccessor(
            IDistributedCache<TenantCacheItem> cache,
            ICurrentTenant currentTenant,
            IRepository<TenantExt> tenantExtRepository,
            IRepository<PayParameter> payParameterRepository
            )
        {
            _cache = cache;
            _currentTenant = currentTenant;
            _tenantExtRepository = tenantExtRepository;
            _payParameterRepository = payParameterRepository;
            _currentScope = new AsyncLocal<TenantCacheItem>();
        }

        public TenantCacheItem Current
        {
            get
            {
                var task = GetTenantCacheItem();
                task.Wait();

                _currentScope.Value = task.Result;

                return _currentScope.Value;
            }
            set { _currentScope.Value = value; }
        }

        public async Task<TenantCacheItem> GetTenantCacheItem()
        {
            if (_currentScope != null && _currentScope.Value != null) return _currentScope.Value;

            var item = await _cache.GetOrAddAsync(
                GetKey(),
                async () =>
                {
                    var tenantExt = await _tenantExtRepository.SingleOrDefaultAsync();
                    CheckHelper.IsNotNull(tenantExt, name: nameof(tenantExt));

                    return new TenantCacheItem
                    {
                        SoftwareCode = tenantExt.SoftwareCode,
                        PaypameterVersion = tenantExt.PaypameterVersion,
                    };
                },
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) //TODO: Should be configurable.
                });

            return item;
        }

        public async Task SetParameterVersion(long version)
        {
            var isContainVersion = await _payParameterRepository.AnyAsync(p => p.Version == version);
            CheckHelper.IsTrue(isContainVersion, $"无效的参数版本：{version}");

            var tenantExt = await _tenantExtRepository.SingleOrDefaultAsync();
            tenantExt.PaypameterVersion = version;

            await _tenantExtRepository.UpdateAsync(tenantExt);

            var item = new TenantCacheItem
            {
                SoftwareCode = tenantExt.SoftwareCode,
                PaypameterVersion = version,
            };
            await _cache.SetAsync(
                GetKey(),
                item,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            _currentScope.Value = item;
        }

        private string GetKey()
        {
            if (_currentTenant.IsAvailable) return _currentTenant.Id.Value.ToString("N");

            return Guid.Empty.ToString("N");
        }
    }
}
