using Adee.Store.CallbackRequests;
using Adee.Store.Pays;
using Adee.Store.Products;
using Adee.Store.Wechats.Components;
using Adee.Store.Wechats.Components.Models;
using Adee.Store.Wechats.Components.Repositorys;
using Adee.Store.Wechats.OffiAccount;
using Adee.Store.Wechats.OffiAccount.Messages.Repositorys;
using AutoMapper;
using SKIT.FlurlHttpClient.Wechat.Api.Events;
using System.Collections.Generic;
using System.Linq;

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

            CreateMap<C2BPayTaskDto, C2B>();

            CreateMap<JSApiPayTaskDto, JSApi>();

            CreateMap<Product, ProductDto>()
                .ForMember(p => p.Specs, config => config.MapFrom(src => src.Specs.AsObject<List<ProductSpec>>(null)));

            CreateMap<CreateUpdateProductDto, Product>()
                .ForMember(p => p.Specs, config => config.MapFrom(src => src.Specs.ToJsonString(null)));

            CreateMap<ProductCatalog, ProductCatalogDto>();
            CreateMap<CreateUpdateProductCatalogDto, ProductCatalog>();

            CreateMap<ProductStock, ProductStockDto>();
            CreateMap<CreateUpdateProductStockDto, ProductStock>();

            CreateMap<ProductStockLog, ProductStockLogDto>();
            CreateMap<CreateUpdateProductStockLogDto, ProductStockLog>();

            CreateMap<ProductSale, ProductSaleDto>();
            CreateMap<CreateUpdateProductSaleDto, ProductSale>();

            CreateMap<ProductStockOrder, ProductStockOrderDto>();
            CreateMap<CreateUpdateProductStockOrderDto, ProductStockOrder>();

            CreateMap<AuthUrlDto, AuthUrl>();

            CreateMap<AssertNotifyRequest, CallbackRequest>()
                .ForMember(p => p.Header, config => config.MapFrom(src => src.Headers.ToJsonString(null)));
            CreateMap<Request, CallbackRequest>();

            CreateMap<ComponentVerifyTicketEvent, ComponentVerifyTicketCacheItem>();

            CreateMap<WechatComponentConfig, ComponentConfigCacheItem>()
                .ForMember(p => p.AppId, config => config.MapFrom(src => src.ComponentAppId));

            CreateMap<WechatOffiAccoutReplyMessage, ReplyMessageDto>();
            CreateMap<CreateUpdateReplyMessageDto, WechatOffiAccoutReplyMessage>();
        }
    }
}
