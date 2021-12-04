using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Auditing;

namespace Adee.Store.NotificationManagement.Hubs
{
    /// <summary>
    /// SignalRÏûÏ¢Hub
    /// </summary>
    [HubRoute("SignalR/Notification")]
    [Authorize]
    [DisableAuditing]
    public class NotificationHub : AbpHub<INotificationHub>
    {
        
    }
}