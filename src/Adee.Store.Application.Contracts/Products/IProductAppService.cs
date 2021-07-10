using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品服务接口
    /// </summary>
    public interface IProductAppService : ICrudAppService<ProductDto, Guid, ProductListDto, CreateUpdateProductDto>
    {

    }
}
