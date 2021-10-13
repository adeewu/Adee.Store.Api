﻿using Adee.Store.Pays;
using AutoMapper;

namespace Adee.Store
{
    public class StoreApplicationAutoMapperProfile : Profile
    {
        public StoreApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<PayParameter, PayParameterDto>();
            CreateMap<CreateUpdatePayParameterDto, PayParameter>();

            CreateMap<PayOrder, QueryOrderCacheItem>();
            CreateMap<PayOrder, OrderResult>();

            CreateMap<QueryOrderCacheItem, OrderResult>();

            CreateMap<B2CPayTaskDto, B2C>();

            CreateMap<JSApiPayTaskDto, JSApi>();
        }
    }
}
