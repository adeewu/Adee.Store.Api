using AutoMapper;
using Adee.Store.NotificationManagement.Notifications.Etos;

namespace Adee.Store.NotificationManagement.Notifications
{
    public class NotificationDomainAutoMapperProfile:Profile
    {
        public NotificationDomainAutoMapperProfile()
        {
            CreateMap<Notification, NotificationEto>();
            CreateMap<NotificationSubscription, NotificationSubscriptionEto>();
        }
    }
}