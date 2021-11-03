using Adee.Store.Pays;
using Adee.Store.Wechats.Components.Repositorys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore
{
    public partial class StoreDbContext
    {
        public virtual DbSet<WechatComponentAuth> WechatComponentAuth { get; set; }

        public void ConfigureWechatComponent(ModelBuilder builder)
        {
            builder.Entity<WechatComponentAuth>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(WechatComponentAuth) + "s", StoreConsts.DbSchema);
                entity.ConfigureByConvention();

                entity.HasComment("第三方平台授权");

                entity.Property(e => e.ComponentAppId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("第三方平台AppId");

                entity.Property(e => e.AuthAppId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("授权AppId");

                entity.Property(e => e.AuthorizationCode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("授权码");

                entity.Property(e => e.AuthorizerRefreshToken)
                    .HasMaxLength(200)
                    .HasComment("刷新令牌");

                entity.Property(e => e.FuncInfo)
                    .HasComment("授权权限集");
            });
        }
    }
}
