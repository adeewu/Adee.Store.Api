using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Adee.Store.EntityFrameworkCore
{
    [DependsOn(
        typeof(StoreEntityFrameworkCoreModule)
        )]
    public class StoreEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<StoreMigrationsDbContext>();
        }
    }
}
