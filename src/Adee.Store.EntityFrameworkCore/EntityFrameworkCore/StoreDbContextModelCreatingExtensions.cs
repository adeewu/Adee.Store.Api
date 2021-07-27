using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Adee.Store.EntityFrameworkCore
{
    public static class StoreDbContextModelCreatingExtensions
    {
        public static void ConfigureStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            ModelCreatings.Pay.ConfigureStore(builder);
            ModelCreatings.Order.ConfigureStore(builder);
            ModelCreatings.Product.ConfigureStore(builder);
        }
    }
}