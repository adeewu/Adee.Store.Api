using Adee.Store.Domain.Tenants;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.TenantManagement;

namespace Adee.Store.Domain.Handlers
{
    public class CreateTenantHandler : ILocalEventHandler<EntityCreatingEventData<Tenant>>, ITransientDependency
    {
        private readonly IRepository<TenantExt> _tenantExtRepository;

        public CreateTenantHandler(IRepository<TenantExt> tenantExtRepository)
        {
            _tenantExtRepository = tenantExtRepository;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<Tenant> eventData)
        {
            var r1 = new Random(GetHashCode());
            var r2 = new Random(GetHashCode() + 1);

            var softwareCode = r1.Next(10000, 99999).ToString() + r2.Next(10000, 99999).ToString();

            var existSoftwareCode = await _tenantExtRepository.AnyAsync(p => p.SoftwareCode == softwareCode);
            CheckHelper.IsFalse(existSoftwareCode, $"软件编号：{softwareCode}已存在");

            await _tenantExtRepository.InsertAsync(new TenantExt
            {
                TenantId = eventData.Entity.Id,
                SoftwareCode = softwareCode,
            });

            await Task.CompletedTask;
        }
    }
}
