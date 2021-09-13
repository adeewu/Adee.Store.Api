using System;
using System.Collections.Generic;
using System.Text;
using Adee.Store.Domain.Shared.Tenants;
using Adee.Store.Localization;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;

namespace Adee.Store
{
    /* Inherit your application services from this class.
     */
    public abstract class StoreAppService : ApplicationService
    {
        protected ICurrentTenantExt CurrentTenantExt
        {
            get
            {
                return LazyServiceProvider.LazyGetRequiredService<ICurrentTenantExt>();
            }
        }

        protected IHttpContextAccessor HttpContextAccessor
        {
            get
            {
                return LazyServiceProvider.LazyGetRequiredService<IHttpContextAccessor>();
            }
        }

        protected StoreAppService()
        {
            LocalizationResource = typeof(StoreResource);
        }

        /// <summary>
        /// 获取访问域名
        /// </summary>
        /// <returns></returns>
        protected string CurrentDomain
        {
            get
            {
                if (HttpContextAccessor.HttpContext == null) return string.Empty;

                var request = HttpContextAccessor.HttpContext.Request;
                if (request.Headers.ContainsKey("Origin"))
                {
                    return request.Headers["Origin"].ToString();
                }
                if (request.Headers.ContainsKey("X-From-Where"))
                {
                    return request.Headers["X-From-Where"];
                }

                var host = $"{request.Scheme}://{request.Host.Host}";
                if (request.Host.Port.HasValue)
                {
                    host += ":" + request.Host.Port;
                }

                return host;
            }
        }
    }
}
