using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Adee.Store.Products
{
    public interface IProductStockAppService : ICrudAppService<ProductStockDto, Guid, ProductStockListDto, CreateUpdateProductStockDto>
    {
        Task<Dictionary<string, decimal>> GetSpecs(Guid productId);
    }
}
