using Adee.Store.Utils.Extensions;
using Adee.Store.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品入库
    /// </summary>
    public class ProductStockOrderAppService : CrudAppService<ProductStockOrder, ProductStockOrderDto, Guid, ProductStockOrderListDto, CreateUpdateProductStockOrderDto>, IProductStockOrderAppService
    {
        private readonly IRepository<ProductStock, Guid> _productStockRepository;
        private readonly IRepository<ProductStockLog, Guid> _productStockLogRepository;
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductStockOrderAppService(
            IRepository<ProductStockOrder, Guid> repository,
            IRepository<ProductStock, Guid> productStockRepository,
            IRepository<ProductStockLog, Guid> productStockLogRepository,
            IRepository<Product, Guid> productRepository)
            : base(repository)
        {
            _productStockRepository = productStockRepository;
            _productStockLogRepository = productStockLogRepository;
            _productRepository = productRepository;
        }

        public override Task<ProductStockOrderDto> UpdateAsync(Guid id, CreateUpdateProductStockOrderDto input)
        {
            throw new UserFriendlyException("入库单不允许修改");
        }

        public override async Task<ProductStockOrderDto> CreateAsync(CreateUpdateProductStockOrderDto input)
        {
            var predicate = PredicateBuilderExtension.False<ProductStock>();
            input.Products.ForEach(product =>
            {
                predicate = predicate.OrCondition(p => p.ProductId == product.ProductId && p.Spec == product.Spec);
            });

            var stocks = await AsyncExecuter.ToListAsync(_productStockRepository.Where(predicate));

            foreach (var productItem in input.Products)
            {
                var stock = stocks.Where(p => p.ProductId == productItem.ProductId).Where(p => p.Spec == productItem.Spec).FirstOrDefault();
                if (stock == null)
                {
                    var product = await AsyncExecuter.SingleOrDefaultAsync(_productRepository.Where(p => p.Id == productItem.ProductId));
                    stock = await _productStockRepository.InsertAsync(new ProductStock(GuidGenerator.Create())
                    {
                        ProductId = productItem.ProductId,
                        Quantity = productItem.Quantity,
                        Spec = productItem.Spec,
                        TenantId = CurrentTenant.Id,
                    }, autoSave: true);
                }
                else
                {
                    stock.Quantity += productItem.Quantity;
                }

                var log = new ProductStockLog(GuidGenerator.Create())
                {
                    ProductStockId = stock.Id,
                    CostPrice = productItem.CostPrice,
                    Quantity = productItem.Quantity,
                    BatchNo = input.BatchNo,
                    OriginPlace = productItem.OriginPlace,
                    Source = input.Supplier,
                };
                await _productStockLogRepository.InsertAsync(log, autoSave: true);
            }

            return await base.CreateAsync(input);
        }

        public async Task<List<ProductStockLogDto>> GetProductsAsync(string batchNo)
        {
            var stockLogs = await AsyncExecuter.ToListAsync(_productStockLogRepository.Where(p => p.BatchNo == batchNo).Select(p => new ProductStockLogDto
            {
                Id = p.Id,
                BatchNo = p.BatchNo,
                CostPrice = p.CostPrice,
                ProductName = p.ProductStock.Product.Name,
                OriginPlace = p.OriginPlace,
                ProductId = p.ProductStock.ProductId,
                Quantity = p.Quantity,
                TenantId = p.ProductStock.TenantId,
                Spec = p.ProductStock.Spec,
            }));

            return stockLogs;
        }
    }
}
