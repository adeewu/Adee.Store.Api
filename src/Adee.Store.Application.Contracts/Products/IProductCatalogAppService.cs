using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品分类接口
    /// </summary>
    public interface IProductCatalogAppService : ICrudAppService<ProductCatalogDto, Guid, ProductCatalogListDto, CreateUpdateProductCatalogDto>
    {
        /// <summary>
        /// 获取分类树形集合
        /// </summary>
        /// <param name="parentCatalogId">父类Id，留空为根分类</param>
        /// <param name="isOnlyFirstChild">仅仅第一级节点</param>
        /// <returns></returns>
        Task<List<ProductCatalogDto>> GetTreeAsync(Guid parentCatalogId, bool isOnlyFirstChild = true);
    }
}
