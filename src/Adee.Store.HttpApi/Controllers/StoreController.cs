using Adee.Store.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Adee.Store.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class StoreController : AbpController
    {
        protected StoreController()
        {
            LocalizationResource = typeof(StoreResource);
        }
    }
}