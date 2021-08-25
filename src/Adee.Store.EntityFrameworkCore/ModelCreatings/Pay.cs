using Adee.Store.Pays;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore
{
    public partial class StoreDbContext
    {
        public virtual DbSet<PayNotify> PayNotify { get; set; }

        public virtual DbSet<PayOrder> PayOrder { get; set; }

        public virtual DbSet<PayOrderLog> PayOrderLog { get; set; }

        public virtual DbSet<PayParameter> PayParameter { get; set; }

        public virtual DbSet<PayRefund> PayRefund { get; set; }

        public void ConfigurePay(ModelBuilder builder)
        {
            builder.Entity<PayNotify>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(PayNotify));
                entity.ConfigureByConvention();

                entity.HasComment("支付回调通知");

                entity.Property(e => e.Body).HasComment("请求正文");

                entity.Property(e => e.HashCode)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("Method、Url、Body、Query经过MD5计算的值");

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("请求方式");

                entity.Property(e => e.PayOrderId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("支付订单Id");

                entity.Property(e => e.Query)
                    .HasComment("请求参数");

                entity.Property(e => e.MerchantOrderId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("支付结果查询Id");

                entity.Property(e => e.ResultStatus)
                    .IsRequired()
                    .HasComment("通知内容状态");

                entity.Property(e => e.ResultStatusMessage)
                    .HasMaxLength(500)
                    .HasComment("通知内容状态描述");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasComment("通知执行状态");

                entity.Property(e => e.StatusMessage)
                    .HasMaxLength(50)
                    .HasComment("通知状态状态描述");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasComment("通知地址");
            });

            builder.Entity<PayOrder>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(PayOrder));
                entity.ConfigureByConvention();

                entity.HasComment("支付订单");

                entity.Property(e => e.BusinessType)
                    .IsRequired()
                    .HasComment("业务模块类型");

                entity.Property(e => e.Money)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasComment("收款金额");

                entity.Property(e => e.NotifyStatus)
                    .IsRequired()
                    .HasComment("通知状态");

                entity.Property(e => e.NotifyStatusMessage)
                    .HasMaxLength(50)
                    .HasComment("通知状态描述");

                entity.Property(e => e.NotifyUrl)
                    .IsRequired()
                    .HasComment("通知地址");

                entity.Property(e => e.OrderData).HasComment("订单数据");

                entity.Property(e => e.OrderTime)
                    .HasComment("订单时间");

                entity.Property(e => e.ParameterVersion)
                    .IsRequired()
                    .HasComment("支付参数版本");

                entity.Property(e => e.PayOrderId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("支付Id");

                entity.Property(e => e.PayOrganizationOrderId)
                    .HasMaxLength(128)
                    .HasComment("收单机构订单号");

                entity.Property(e => e.PayOrganizationType)
                    .IsRequired()
                    .HasComment("收单机构");

                entity.Property(e => e.PaymentType)
                    .IsRequired()
                    .HasComment("付款方式");

                entity.Property(e => e.PaymethodType)
                    .IsRequired()
                    .HasComment("支付方式");

                entity.Property(e => e.MerchantOrderId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("支付结果查询Id");

                entity.Property(e => e.QueryStatus)
                    .IsRequired()
                    .HasComment("查询状态");

                entity.Property(e => e.QueryStatusMessage)
                    .HasMaxLength(50)
                    .HasComment("查询状态描述");

                entity.Property(e => e.RefundCount).HasComment("成功退款次数");

                entity.Property(e => e.RefundStatus).HasComment("退款状态");

                entity.Property(e => e.RefundStatusMessage)
                    .HasMaxLength(50)
                    .HasComment("退款状态描述");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasComment("支付状态");

                entity.Property(e => e.StatusMessage)
                    .HasMaxLength(50)
                    .HasComment("支付状态描述");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("收款标题");

                entity.Property(e => e.TargetDomain)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .HasComment("发起支付域名");
            });

            builder.Entity<PayOrderLog>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(PayOrderLog));
                entity.ConfigureByConvention();

                entity.HasComment("支付订单记录");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("订单Id");

                entity.Property(e => e.ExceptionMessage).HasComment("异常描述");

                entity.Property(e => e.LogData).HasComment("记录数据");

                entity.Property(e => e.LogType)
                    .IsRequired()
                    .HasComment("记录类型");

                entity.Property(e => e.Message)
                    .HasMaxLength(50)
                    .HasComment("描述");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasComment("记录状态");

                entity.HasOne(e => e.PayOrder)
                    .WithMany(e => e.PayOrderLogs)
                    .HasForeignKey(e => e.OrderId);
            });

            builder.Entity<PayParameter>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(PayParameter));
                entity.ConfigureByConvention();

                entity.HasComment("支付参数");

                entity.Property(e => e.PayOrganizationType)
                    .IsRequired()
                    .HasComment("收单机构");

                entity.Property(e => e.PaymentType).HasComment("付款方式");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasComment("支付参数值");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasComment("版本");
            });

            builder.Entity<PayRefund>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(PayRefund));
                entity.ConfigureByConvention();

                entity.HasComment("支付订单退款记录");

                entity.Property(e => e.Money)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasComment("退款金额");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("订单Id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasComment("退款状态");

                entity.Property(e => e.StatusMessage)
                    .HasMaxLength(500)
                    .HasComment("退款状态描述");

                entity.HasOne(e => e.PayOrder)
                    .WithMany(e => e.PayRefunds)
                    .HasForeignKey(e => e.OrderId);
            });
        }
    }
}
