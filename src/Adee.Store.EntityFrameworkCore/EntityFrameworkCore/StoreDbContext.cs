using Microsoft.EntityFrameworkCore;
using Adee.Store.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;
using Adee.Store.Orders;
using Adee.Store.Products;

namespace Adee.Store.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See StoreMigrationsDbContext for migrations.
     */
    [ConnectionStringName("Default")]
    public class StoreDbContext : AbpDbContext<StoreDbContext>
    {
        public DbSet<AppUser> Users { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<OrderInfo> OrderDetail { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<ProductSale> ProductSale { get; set; }

        public virtual DbSet<ProductSaleInfo> ProductSaleInfo { get; set; }

        public virtual DbSet<ProductStockOrder> ProductStockOrder { get; set; }

        public virtual DbSet<ProductStock> ProductStock { get; set; }

        public virtual DbSet<ProductStockLog> ProductStockLog { get; set; }

        public virtual DbSet<ProductCatalog> ProductCatalog { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser
                
                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                /* Configure mappings for your additional properties
                 * Also see the StoreEfCoreEntityExtensionMappings class
                 */
            });

            /* Configure your own tables/entities inside the ConfigureStore method */

            builder.ConfigureStore();
        }
    }
}
