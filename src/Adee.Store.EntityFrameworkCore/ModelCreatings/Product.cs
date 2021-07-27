using Adee.Store.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore.ModelCreatings
{
    public class Product
    {
        public static void ConfigureStore(ModelBuilder builder)
        {
            builder.Entity<Products.Product>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(Product));
                entity.ConfigureByConvention();

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("条码");

                entity.Property(e => e.CatalogId).HasComment("产品分类");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("名称");

                entity.Property(e => e.Photo)
                    .HasMaxLength(255)
                    .HasComment("图片");

                entity.Property(e => e.PricingType).HasComment("计价方式，1：计件，2：计重");

                entity.Property(e => e.ProductBrand)
                    .HasMaxLength(50)
                    .HasComment("品牌");

                entity.Property(e => e.QuickCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("助记码");

                entity.Property(e => e.SaleVolume)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("销量，冗余值");

                entity.Property(e => e.Specs)
                    .HasMaxLength(2000)
                    .HasComment("商品规格，单规格商品留空");

                entity.Property(e => e.Stock)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("库存，冗余值");

                entity.Property(e => e.UnitName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("单位名称");

                entity.HasOne(e => e.ProductCatalog)
                    .WithMany()
                    .HasForeignKey(e => e.CatalogId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<ProductSale>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(ProductSale));
                entity.ConfigureByConvention();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("售卖标题");

                entity.Property(e => e.Recommend).HasComment("推荐售卖");

                entity.Property(e => e.MarketPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("市场价");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("售卖单价");

                entity.Property(e => e.SaleVolume)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("售卖量");

                entity.Property(e => e.TotalSaleVolume)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("总售卖量");

                entity.Property(e => e.AllowOversell)
                    .HasComment("允许超售");

                entity.Property(e => e.Discount).HasComment("折扣，百分比");

                entity.Property(e => e.Status).HasComment("状态，1：正常，-1：下架");

                entity.Property(e => e.Description).HasComment("商品描述");

                entity.Property(e => e.Photo).HasComment("图片");

                entity.Property(e => e.ProductSaleType).HasComment("售卖类型");
            });

            builder.Entity<ProductSaleInfo>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(ProductSaleInfo));
                entity.ConfigureByConvention();

                entity.Property(e => e.ProductStockId).HasComment("库存Id");

                entity.Property(e => e.ProductSaleId).HasComment("商品售卖Id");

                entity.Property(e => e.Quantity).HasComment("售卖量");

                entity.HasOne(e => e.ProductSale)
                    .WithMany(e => e.ProductSaleInfos)
                    .HasForeignKey(e => e.ProductSaleId);
            });

            builder.Entity<ProductStockOrder>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(ProductStockOrder));
                entity.ConfigureByConvention();

                entity.Property(e => e.BatchNo)
                    .IsRequired()
                    .HasComment("库存批次号");

                entity.Property(e => e.Supplier)
                    .IsRequired()
                    .HasComment("供应商");

                entity.Property(e => e.Money)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("订单金额");

                entity.Property(e => e.ActualMoney)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("实付金额");

                entity.Property(e => e.Payment)
                    .IsRequired()
                    .HasComment("付款方式");

                entity.Property(e => e.Remark).HasComment("备注");
            });

            builder.Entity<ProductStock>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(ProductStock));
                entity.ConfigureByConvention();

                entity.Property(e => e.ProductId).HasComment("商品Id");

                entity.Property(e => e.Spec).HasComment("商品规格");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("库存");

                entity.Property(e => e.Warranty)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("质保期");

                entity.Property(e => e.WarrantyUnit)
                    .HasMaxLength(10)
                    .HasComment("质保期单位");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductStocks)
                    .HasForeignKey(d => d.ProductId);
            });

            builder.Entity<ProductStockLog>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(ProductStockLog));
                entity.ConfigureByConvention();

                entity.Property(e => e.ProductStockId).HasComment("库存Id");

                entity.Property(e => e.CostPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("进货价");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("进货量");

                entity.Property(e => e.BatchNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("库存批次号");

                entity.Property(e => e.OriginPlace)
                    .HasMaxLength(50)
                    .HasComment("原产地");

                entity.Property(e => e.Source)
                    .HasMaxLength(255)
                    .HasComment("库存来源");

                entity.HasOne(d => d.ProductStock)
                    .WithMany(p => p.ProductStockLogs)
                    .HasForeignKey(d => d.ProductStockId);
            });

            builder.Entity<ProductCatalog>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(ProductCatalog));
                entity.ConfigureByConvention();

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasComment("分类名称");

                entity.Property(e => e.CatalogPath)
                    .HasMaxLength(1000)
                    .HasComment("分类路径，计算值");

                entity.Property(e => e.ParentCatalogId).HasComment("父分类Id");
            });
        }
    }
}
