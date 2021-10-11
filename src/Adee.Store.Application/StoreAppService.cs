using Adee.Store.Domain.Tenants;
using Adee.Store.Localization;
using Volo.Abp.Application.Services;

namespace Adee.Store
{
    /* Inherit your application services from this class.
     */
    public abstract class StoreAppService : ApplicationService
    {
        /// <summary>
        /// 扩展租户信息
        /// </summary>
        protected ICurrentTenantExt CurrentTenantExt
        {
            get
            {
                return LazyServiceProvider.LazyGetRequiredService<ICurrentTenantExt>();
            }
        }

        protected StoreAppService()
        {
            LocalizationResource = typeof(StoreResource);
        }
    }
}
