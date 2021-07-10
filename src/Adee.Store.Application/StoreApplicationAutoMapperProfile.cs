using System.Collections.Generic;
using Adee.Store.Products;
using AutoMapper;
using System.Linq;

namespace Adee.Store
{
    public class StoreApplicationAutoMapperProfile : Profile
    {
        public StoreApplicationAutoMapperProfile()
        {
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
        }
    }
}
