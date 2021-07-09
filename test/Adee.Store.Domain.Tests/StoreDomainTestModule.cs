using Adee.Store.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Adee.Store
{
    [DependsOn(
        typeof(StoreEntityFrameworkCoreTestModule)
        )]
    public class StoreDomainTestModule : AbpModule
    {

    }
}