using Adee.Store.NotificationManagement.Notifications;
using Adee.Store.Wechats.OffiAccount.Messages.Repositorys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore
{
    public partial class StoreDbContext
    {
        public DbSet<Notification> Notification { get; set; }

        public DbSet<NotificationSubscription> NotificationSubscription { get; set; }

        public void ConfigureNotificationManagement(ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Notification>(b =>
            {
                b.ToTable(StoreConsts.DbTablePrefix + nameof(Notification) + "s", StoreConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasComment("消息通知");
            });

            builder.Entity<NotificationSubscription>(b =>
            {
                b.ToTable(StoreConsts.DbTablePrefix + nameof(NotificationSubscription) + "s", StoreConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasComment("消息订阅者");
            });
        }
    }
}
