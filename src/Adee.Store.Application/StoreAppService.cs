using System;
using System.Collections.Generic;
using System.Text;
using Adee.Store.Localization;
using Volo.Abp.Application.Services;

namespace Adee.Store
{
    /* Inherit your application services from this class.
     */
    public abstract class StoreAppService : ApplicationService
    {
        protected StoreAppService()
        {
            LocalizationResource = typeof(StoreResource);
        }
    }
}
