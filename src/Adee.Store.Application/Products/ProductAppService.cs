using Adee.Store.Attributes;
using Adee.Store.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品服务
    /// </summary>
    [ApiGroup(ApiGroupType.Product)]
    //[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(ProductListDto))]
    [Authorize(StorePermissions.Products.Default)]
    public class ProductAppService : CrudAppService<Product, ProductDto, Guid, ProductListDto, CreateUpdateProductDto>, IProductAppService
    {
        private readonly IRepository<ProductCatalog, Guid> _productCatalogRepository;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<ProductCatalog, Guid> productCatalogRepository)
            : base(productRepository)
        {
            _productCatalogRepository = productCatalogRepository;
        }

        protected override async Task<IQueryable<Product>> CreateFilteredQueryAsync(ProductListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            return query.WhereIf(input.CatalogId.HasValue, p => p.CatalogId == input.CatalogId)
                .WhereIf(!input.BarCode.IsNullOrWhiteSpace(), p => p.BarCode == input.BarCode)
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), p => p.Name.Contains(input.Name));
        }

        protected override async Task<List<ProductDto>> MapToGetListOutputDtosAsync(List<Product> entities)
        {
            var result = await base.MapToGetListOutputDtosAsync(entities);

            var catalogIds = entities.Select(p => p.CatalogId);
            var catalogs = await AsyncExecuter.ToListAsync(_productCatalogRepository.Where(p => catalogIds.Contains(p.Id)));

            result.ForEach(p => p.Catalog = catalogs.Where(c => c.Id == p.CatalogId).Select(p => p.Name).FirstOrDefault());
            return result;
        }
    }
}
