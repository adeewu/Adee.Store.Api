using Adee.Store.Attributes;
using Adee.Store.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品库存
    /// </summary>
    [ApiGroup(ApiGroupType.Product)]
    public class ProductStockAppService : CrudAppService<ProductStock, ProductStockDto, Guid, ProductStockListDto, CreateUpdateProductStockDto>, IProductStockAppService
    {
        private readonly IRepository<Product, Guid> _productRespository;

        public ProductStockAppService(
            IRepository<ProductStock, Guid> productStockRespository,
            IRepository<Product, Guid> productRespository)
            : base(productStockRespository)
        {
            _productRespository = productRespository;
        }

        protected override async Task<IQueryable<ProductStock>> CreateFilteredQueryAsync(ProductStockListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);
            return query.WhereIf(!input.Filter.IsNullOrWhiteSpace(), p => p.Product.Name.Contains(input.Filter) || p.Spec.Contains(input.Filter));
        }

        protected override async Task<List<ProductStockDto>> MapToGetListOutputDtosAsync(List<ProductStock> entities)
        {
            var result = await base.MapToGetListOutputDtosAsync(entities);

            var productIds = entities.Select(p => p.ProductId);
            var products = await AsyncExecuter.ToListAsync(_productRespository.Where(p => productIds.Contains(p.Id)));

            result.ForEach(p => p.ProductName = products.Where(e => e.Id == p.ProductId).Select(e => e.Name).FirstOrDefault());

            return result;
        }

        /// <summary>
        /// 获取商品的规格库存
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, decimal>> GetSpecs(Guid productId)
        {
            var stocks = await AsyncExecuter.ToListAsync(Repository.Where(p => p.ProductId == productId).Select(p => new { p.Spec, p.Quantity }));
            if (!stocks.Any()) return new Dictionary<string, decimal>();

            return stocks
                .Select(p => ProductSpecHelper.ToDic(p.Spec).Select(s => new { Spec = $"{s.Key}:{s.Value}", p.Quantity }))
                .SelectMany(p => p)
                .GroupBy(p => p.Spec)
                .Select(p => new
                {
                    Spec = p.Key,
                    Quantity = p.Sum(s => s.Quantity)
                })
                .ToDictionary(p => p.Spec, p => p.Quantity);
        }
    }
}
