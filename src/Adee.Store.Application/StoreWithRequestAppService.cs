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
    public abstract class StoreWithRequestAppService : StoreAppService
    {
        /// <summary>
        /// 请求上下文
        /// </summary>
        protected HttpContext HttpContext
        {
            get
            {
                return LazyServiceProvider.LazyGetRequiredService<IHttpContextAccessor>().HttpContext;
            }
        }

        /// <summary>
        /// 获取访问域名
        /// </summary>
        /// <returns></returns>
        protected string CurrentDomain
        {
            get
            {
                if (HttpContext == null || HttpContext.Request == null) return string.Empty;

                return HttpContext?.Request.GetDomain();
            }
        }
    }
}
