using Adee.Store.Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore
{
    public partial class StoreDbContext
    {
        public virtual DbSet<TenantExt> TenantExt { get; set; }

        public void ConfigureTenant(ModelBuilder builder)
        {
            builder.Entity<TenantExt>(entity =>
            {
                entity.ToTable(Volo.Abp.TenantManagement.AbpTenantManagementDbProperties.DbTablePrefix + nameof(TenantExt) + "s", Volo.Abp.TenantManagement.AbpTenantManagementDbProperties.DbSchema);
                entity.ConfigureByConvention();

                entity.Property(e => e.SoftwareCode).HasComment("软件编号");

                entity.Property(e => e.PaypameterVersion).HasComment("支付参数版本");
            });
        }
    }
}
