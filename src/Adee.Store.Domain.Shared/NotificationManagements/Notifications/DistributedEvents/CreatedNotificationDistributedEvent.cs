using Adee.Store.NotificationManagement.Notifications.Etos;

namespace Adee.Store.NotificationManagement.Notifications.DistributedEvents
{
    public class CreatedNotificationDistributedEvent
    {
        public NotificationEto NotificationEto { get;  set; }

        private CreatedNotificationDistributedEvent()
        {
            
        }

        public CreatedNotificationDistributedEvent(NotificationEto notificationEto)
        {
            NotificationEto = notificationEto;
        }
    }
}