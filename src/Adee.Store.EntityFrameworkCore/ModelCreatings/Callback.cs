using Adee.Store.CallbackRequests;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore
{
    public partial class StoreDbContext
    {
        public virtual DbSet<CallbackRequest> CallbackRequest { get; set; }

        public void ConfigureCallback(ModelBuilder builder)
        {
            builder.Entity<CallbackRequest>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(CallbackRequest) + "s", StoreConsts.DbSchema);
                entity.ConfigureByConvention();

                entity.HasComment("回调请求");

                entity.Property(e => e.Body)
                    .HasComment("请求正文");

                entity.Property(e => e.HashCode)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("Method、Url、Body、Query、Header经过MD5计算的值");

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("请求方式");

                entity.Property(e => e.Query)
                    .HasComment("请求参数");

                entity.Property(e => e.Header)
                    .HasComment("请求头部");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasComment("通知地址");
            });
        }
    }
}
