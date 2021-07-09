using Volo.Abp.Modularity;

namespace Adee.Store
{
    [DependsOn(
        typeof(StoreApplicationModule),
        typeof(StoreDomainTestModule)
        )]
    public class StoreApplicationTestModule : AbpModule
    {

    }
}