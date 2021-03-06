using Adee.Store.Orders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore
{
    public partial class StoreDbContext
    {
        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<OrderInfo> OrderDetail { get; set; }

        public void ConfigureOrder(ModelBuilder builder)
        {
            builder.Entity<Order>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(Order) + "s", StoreConsts.DbSchema);
                entity.ConfigureByConvention();

                entity.HasComment("订单");

                entity.Property(e => e.BusinessType).HasComment("业务类型");

                entity.Property(e => e.TerminalType).HasComment("终端类型");

                entity.Property(e => e.OrderStatus).HasComment("订单状态");

                entity.Property(e => e.OrderTime)
                    .HasComment("订单时间");

                entity.Property(e => e.PayStatus).HasComment("收款状态");

                entity.Property(e => e.PayTime)
                    .HasComment("收款时间");

                entity.Property(e => e.Payment)
                    .HasMaxLength(20)
                    .HasComment("收款方式");

                entity.Property(e => e.RunningId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("订单流水号");

                entity.Property(e => e.MerchantOrderId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("支付业务号");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("订单标题");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("销售数量");

                entity.Property(e => e.Money)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("销售金额");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("销售单价，单一商品有效");
            });

            builder.Entity<OrderInfo>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(OrderInfo) + "s", StoreConsts.DbSchema);
                entity.ConfigureByConvention();

                entity.HasComment("订单详情");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(9,2)")
                    .HasComment("成交数量");

                entity.Property(e => e.OrderId).HasComment("订单Id");

                entity.Property(e => e.OrderType).HasComment("订单类型");

                entity.Property(e => e.DataId).HasComment("数据Id");

                entity.Property(e => e.DataName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("数据名称");

                entity.Property(e => e.Desc).HasComment("成交描述");

                entity.Property(e => e.Money)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("成交总价");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("成交单价");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.OrderId);
            });
        }
    }
}
