using System;
using System.Collections.Generic;
using System.Text;
using Adee.Store.Domain.Shared.Tenants;
using Adee.Store.Localization;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;

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
