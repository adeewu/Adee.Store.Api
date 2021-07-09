using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Adee.Store.EntityFrameworkCore
{
    public static class StoreDbContextModelCreatingExtensions
    {
        public static void ConfigureStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(StoreConsts.DbTablePrefix + "YourEntities", StoreConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}