using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;

namespace Adee.Store.Products
{
    public interface IProductSaleAppService : ICrudAppService<ProductSaleDto, Guid, ProductSaleListDto, CreateUpdateProductSaleDto>
    {
    }
}
