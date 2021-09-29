using Adee.Store.Domain.Tenants;
using Adee.Store.Orders;
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
