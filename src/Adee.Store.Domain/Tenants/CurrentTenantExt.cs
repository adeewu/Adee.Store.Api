using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Domain.Tenants
{
    public class CurrentTenantExt : ICurrentTenantExt, ITransientDependency
    {
        private readonly ICurrentTenantExtAccessor _currentTenantExtAccessor;

        public CurrentTenantExt(ICurrentTenantExtAccessor currentTenantExtAccessor)
        {
            _currentTenantExtAccessor = currentTenantExtAccessor;
        }

        public string SoftwareCode => _currentTenantExtAccessor.Current?.SoftwareCode;

        public long? PaypameterVersion => _currentTenantExtAccessor.Current?.PaypameterVersion;

        public async Task SetPaypameterVersion(long version)
        {
            await _currentTenantExtAccessor.SetParameterVersion(version);
        }
    }
}
