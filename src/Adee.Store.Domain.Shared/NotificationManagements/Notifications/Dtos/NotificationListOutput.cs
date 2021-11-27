using System;

namespace Adee.Store.NotificationManagement.Notifications.Dtos
{
    public class NotificationListOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public bool Read { get; set; }
    }
}