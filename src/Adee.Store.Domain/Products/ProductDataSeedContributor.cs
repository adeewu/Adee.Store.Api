using Adee.Store.Orders;
using Adee.Store.Pays;
using Adee.Store.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Uow;

namespace Adee.Store.Products
{
    public class ProductDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductStock, Guid> _productStockRepository;
        private readonly IRepository<ProductSale, Guid> _productSaleRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<AppUser, Guid> _userRepository;
        private readonly IRepository<ProductCatalog, Guid> _productCatalogRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IJsonSerializer _jsonSerializer;

        public ProductDataSeedContributor(
            IRepository<Product, Guid> productRepository,
            IRepository<ProductStock, Guid> productStockRepository,
            IRepository<ProductSale, Guid> productSaleRepository,
            IRepository<Order, Guid> orderRepository,
            IRepository<AppUser, Guid> userRepository,
            IRepository<ProductCatalog, Guid> productCatalogRepository,
            IGuidGenerator guidGenerator,
            IJsonSerializer jsonSerializer)
        {
            _productRepository = productRepository;
            _productStockRepository = productStockRepository;
            _productSaleRepository = productSaleRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productCatalogRepository = productCatalogRepository;
            _guidGenerator = guidGenerator;
            _jsonSerializer = jsonSerializer;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var admin = await _userRepository.FindAsync(p => p.UserName == "admin");
            Check.NotNull(admin, nameof(admin));

            var batchNo = DateTime.Now.ToString("yyyyMMdd") + "001";

            var productCatalog = new ProductCatalog(_guidGenerator.Create())
            {
                Name = "商品分类1",
                CatalogPath = "商品分类1",
                TenantId = admin.TenantId,
                ParentCatalogId = Guid.Empty,
            };
            var subProductCatalog = new ProductCatalog(_guidGenerator.Create())
            {
                Name = "商品分类1子分类1",
                CatalogPath = productCatalog.Name + "-商品分类1子分类1",
                ParentCatalogId = productCatalog.Id,
                TenantId = admin.TenantId,
            };
            await _productCatalogRepository.InsertAsync(productCatalog, autoSave: true);
            await _productCatalogRepository.InsertAsync(subProductCatalog, autoSave: true);

            #region 单规格商品+销售+订单
            var product1 = new Product(_guidGenerator.Create())
            {
                CatalogId = subProductCatalog.Id,
                CreatorId = admin.Id,
                BarCode = "123456",
                Name = "单个商品",
                PricingType = PricingType.Piece,
                ProductBrand = "品牌名称",
                QuickCode = "DGSP",
                Stock = 40,
                UnitName = "包",
                SaleVolume = 10,
                TenantId = admin.TenantId,
            };
            await _productRepository.InsertAsync(product1, autoSave: true);

            var stock1 = new ProductStock(_guidGenerator.Create())
            {
                ProductId = product1.Id,
                Quantity = 50,
                Warranty = 120,
                WarrantyUnit = "天",
                TenantId = admin.TenantId,
                ProductStockLogs = new List<ProductStockLog>
                {
                    new ProductStockLog(_guidGenerator.Create())
                    {
                        BatchNo = batchNo,
                        CostPrice = 20,
                        OriginPlace = "中国广州",
                        Source = "供应商A",
                        Quantity = 50,
                    }
                }
            };
            await _productStockRepository.InsertAsync(stock1, autoSave: true);

            var sale1 = new ProductSale(_guidGenerator.Create())
            {
                TenantId = admin.TenantId,
                CreatorId = admin.Id,
                ProductSaleType = ProductSaleType.Product,
                Title = product1.Name,
                TotalSaleVolume = stock1.Quantity,
                Description = "销售描述",
                MarketPrice = 30,
                UnitPrice = 28,
                SaleVolume = 10,
                Status = ProductSaleStatus.Selling,
                ProductSaleInfos = new List<ProductSaleInfo>
                {
                    new ProductSaleInfo(_guidGenerator.Create())
                    {
                        ProductStockId = stock1.Id,
                        Quantity = 1,
                    }
                }
            };
            await _productSaleRepository.InsertAsync(sale1, autoSave: true);

            var order1 = new Order(_guidGenerator.Create())
            {
                CreatorId = admin.Id,
                OrderStatus = ExcutingStatus.Success,
                OrderTime = DateTime.Now,
                Payment = "现金",
                PayStatus = ExcutingStatus.Success,
                PayTime = DateTime.Now,
                RunningId = "001",
                MerchantOrderId = new MerchantOrderId().ToString(),
                TenantId = admin.TenantId,
                Title = product1.Name,
                Quantity = 10,
                Money = sale1.UnitPrice * 10,
                UnitPrice = sale1.UnitPrice,
                BusinessType = BusinessType.WebCheckout,
                OrderInfos = new List<OrderInfo>
                {
                    new OrderInfo(_guidGenerator.Create())
                    {
                        CreatorId = admin.Id,
                        Quantity = 10,
                        DataId = product1.Id,
                        DataName = product1.Name,
                        Desc = product1.Specs,
                        TenantId = admin.TenantId,
                        Money = sale1.UnitPrice * 10,
                        UnitPrice = sale1.UnitPrice,
                        OrderType = OrderType.Product,
                    }
                }
            };
            await _orderRepository.InsertAsync(order1, autoSave: true);
            #endregion

            #region 多规格商品+销售+订单
            var productSpecs = new List<ProductSpec>
            {
                new ProductSpec
                {
                    Name = "规格",
                    Order = 1,
                    SubProductSpecs = new List<ProductSpec>
                    {
                        new ProductSpec
                        {
                            Name = "约700毫升",
                            Order = 1,
                        }
                    }
                },
                new ProductSpec
                {
                    Name = "甜度",
                    Order = 1,
                    SubProductSpecs = new List<ProductSpec>
                    {
                        new ProductSpec
                        {
                            Name = "全糖",
                            Order = 1,
                        },
                        new ProductSpec
                        {
                            Name = "少糖",
                            Order = 2,
                        },
                        new ProductSpec
                        {
                            Name = "半糖",
                            Order = 3,
                        },
                        new ProductSpec
                        {
                            Name = "微糖",
                            Order = 4,
                        },
                        new ProductSpec
                        {
                            Name = "无糖",
                            Order = 5,
                        },
                    }
                },
            };

            var multProduct = new Product(_guidGenerator.Create())
            {
                CatalogId = subProductCatalog.Id,
                CreatorId = admin.Id,
                BarCode = "1234567",
                Name = "多规格商品",
                PricingType = PricingType.Piece,
                ProductBrand = "品牌名称",
                QuickCode = "DGGSP",
                Stock = 40,
                UnitName = "包",
                SaleVolume = 10,
                Specs = _jsonSerializer.Serialize(productSpecs),
                TenantId = admin.TenantId,
            };
            await _productRepository.InsertAsync(multProduct, autoSave: true);

            var fullSugarStock = new ProductStock(_guidGenerator.Create())
            {
                ProductId = multProduct.Id,
                Spec = "规格:约700毫升|甜度:全糖",
                Quantity = 30,
                Warranty = 48,
                WarrantyUnit = "小时",
                TenantId = admin.TenantId,
                ProductStockLogs = new List<ProductStockLog>
                {
                    new ProductStockLog(_guidGenerator.Create())
                    {
                        BatchNo = batchNo,
                        CostPrice = 10,
                        OriginPlace = "广州增城",
                        Source = "供应商B",
                    }
                }
            };
            await _productStockRepository.InsertAsync(fullSugarStock, autoSave: true);

            var noneSugarStock = new ProductStock(_guidGenerator.Create())
            {
                ProductId = multProduct.Id,
                Spec = "规格:约700毫升|甜度:无糖",
                Quantity = 20,
                Warranty = 48,
                WarrantyUnit = "小时",
                TenantId = admin.TenantId,
                ProductStockLogs = new List<ProductStockLog>
                {
                    new ProductStockLog(_guidGenerator.Create())
                    {
                        BatchNo = batchNo,
                        CostPrice = 10,
                        OriginPlace = "广州增城",
                        Source = "供应商B",
                    }
                }
            };
            await _productStockRepository.InsertAsync(noneSugarStock, autoSave: true);

            var fullSugarSale = new ProductSale(_guidGenerator.Create())
            {
                Title = $"{multProduct.Name}【{fullSugarStock.Spec}】",
                TotalSaleVolume = fullSugarStock.Quantity,
                CreatorId = admin.Id,
                Description = "销售描述",
                MarketPrice = 20,
                UnitPrice = 18,
                SaleVolume = 6,
                Status = ProductSaleStatus.Selling,
                TenantId = admin.TenantId,
                ProductSaleType = ProductSaleType.Product,
                ProductSaleInfos = new List<ProductSaleInfo>
                {
                    new ProductSaleInfo(_guidGenerator.Create())
                    {
                        ProductStockId = fullSugarStock.Id,
                        Quantity = 1,
                    }
                }
            };
            await _productSaleRepository.InsertAsync(fullSugarSale);

            var noneSugarSale = new ProductSale(_guidGenerator.Create())
            {
                Title = $"{multProduct.Name}【{noneSugarStock.Spec}】",
                TotalSaleVolume = noneSugarStock.Quantity,
                CreatorId = admin.Id,
                Description = "销售描述",
                MarketPrice = 22,
                UnitPrice = 20,
                SaleVolume = 4,
                Status = ProductSaleStatus.Selling,
                TenantId = admin.TenantId,
                ProductSaleType = ProductSaleType.Product,
                ProductSaleInfos = new List<ProductSaleInfo>
                {
                    new ProductSaleInfo(_guidGenerator.Create())
                    {
                        ProductStockId = noneSugarStock.Id,
                        Quantity = 1,
                    }
                }
            };
            await _productSaleRepository.InsertAsync(noneSugarSale);

            var fullOrder = new Order(_guidGenerator.Create())
            {
                CreatorId = admin.Id,
                OrderStatus = ExcutingStatus.Success,
                OrderTime = DateTime.Now,
                Payment = "支付宝",
                PayStatus = ExcutingStatus.Success,
                PayTime = DateTime.Now,
                RunningId = "001",
                MerchantOrderId = new MerchantOrderId().ToString(),
                TenantId = admin.TenantId,
                Title = multProduct.Name,
                Quantity = 10,
                Money = fullSugarSale.UnitPrice * 6 + noneSugarSale.UnitPrice * 4,
                OrderInfos = new List<OrderInfo>
                {
                    new OrderInfo(_guidGenerator.Create())
                    {
                        Quantity = 6,
                        DataId = multProduct.Id,
                        DataName = multProduct.Name,
                        Desc = fullSugarStock.Spec,
                        TenantId = admin.TenantId,
                        Money = fullSugarSale.UnitPrice * 6,
                        UnitPrice = fullSugarSale.UnitPrice,
                        OrderType = OrderType.Product,
                    },
                    new OrderInfo(_guidGenerator.Create())
                    {
                        Quantity = 4,
                        DataId = multProduct.Id,
                        DataName = multProduct.Name,
                        Desc = noneSugarStock.Spec,
                        TenantId = admin.TenantId,
                        Money = noneSugarSale.UnitPrice * 4,
                        UnitPrice = noneSugarSale.UnitPrice,
                        OrderType = OrderType.Product,
                    },
                }
            };
            await _orderRepository.InsertAsync(fullOrder, autoSave: true);
            #endregion
        }
    }
}
