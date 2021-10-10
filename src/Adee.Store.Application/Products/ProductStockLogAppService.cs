using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adee.Store.Attributes;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Products
{
    /// <summary>
    /// 库存记录
    /// </summary>
    [ApiGroup(ApiGroupType.Product)]
    public class ProductStockLogAppService : ReadOnlyAppService<ProductStockLog, ProductStockLogDto, Guid, ProductStockLogListDto>
    {
        public ProductStockLogAppService(IRepository<ProductStockLog, Guid> repository) : base(repository)
        {

        }

        protected override async Task<IQueryable<ProductStockLog>> CreateFilteredQueryAsync(ProductStockLogListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            return query
               .Where(p => p.ProductStock.ProductId == input.ProductId)
               .WhereIf(!input.Filter.IsNullOrWhiteSpace(), p => p.OriginPlace.Contains(input.Filter) || p.BatchNo.Contains(input.Filter))
               .WhereIf(input.StartDate.HasValue && input.EndDate.HasValue, p => p.CreationTime.Date >= input.StartDate.Value.Date && p.CreationTime.Date <= input.EndDate.Value.Date);
        }
    }
}
