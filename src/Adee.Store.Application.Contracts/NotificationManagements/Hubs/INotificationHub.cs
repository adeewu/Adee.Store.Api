using System.Threading.Tasks;
using Adee.Store.NotificationManagement.Notifications.Dtos;

namespace Adee.Store.NotificationManagement.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotificationHub
    {
        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ReceiveTextMessageAsync(SendNotificationDto input);

        /// <summary>
        /// 发送广播消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ReceiveBroadCastMessageAsync(SendNotificationDto input);
    }
}