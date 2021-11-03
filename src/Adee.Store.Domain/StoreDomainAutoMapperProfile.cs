using Adee.Store.CallbackRequests;
using Adee.Store.Pays;
using Adee.Store.Products;
using Adee.Store.Wechats.Components;
using Adee.Store.Wechats.Components.Models;
using AutoMapper;
using SKIT.FlurlHttpClient.Wechat.Api.Events;
using System.Collections.Generic;
using System.Linq;

namespace Adee.Store
{
    public class StoreDomainAutoMapperProfile : Profile
    {
        public StoreDomainAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<ComponentVerifyTicketEvent, ComponentVerifyTicketCacheItem>();
        }
    }
}
