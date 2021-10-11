using System;
using Volo.Abp.Application.Services;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品入库服务接口
    /// </summary>
    public interface IProductStockOrderAppService : ICrudAppService<ProductStockOrderDto, Guid, ProductStockOrderListDto, CreateUpdateProductStockOrderDto>
    {

    }
}
