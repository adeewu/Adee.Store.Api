using System;
using Volo.Abp.Application.Services;

namespace Adee.Store.Products
{
    public interface IProductSaleAppService : ICrudAppService<ProductSaleDto, Guid, ProductSaleListDto, CreateUpdateProductSaleDto>
    {
    }
}
