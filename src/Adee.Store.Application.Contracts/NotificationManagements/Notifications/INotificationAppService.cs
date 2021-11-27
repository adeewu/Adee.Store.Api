using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adee.Store.NotificationManagement.Notifications.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Adee.Store.NotificationManagement.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        /// <returns></returns>
        Task SendMessageAsync(string title, string content, MessageType messageType, List<string> users);

        /// <summary>
        /// 消息设置为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task MarkReadAsync(MarkReadInput input);

        /// <summary>
        /// 创建一个消息
        /// 测试使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAsync(CreateNotificationInput input);

        /// <summary>
        /// 分页获取用户普通文本消息
        /// </summary>
        /// <param name="listInput"></param>
        /// <returns></returns>
        Task<PagedResultDto<NotificationListOutput>> GetTextNotificationByUserIdAsync(NotificationListInput listInput);

        /// <summary>
        /// 分页获取广播消息
        /// </summary>
        /// <param name="listInput"></param>
        /// <returns></returns>
        Task<PagedResultDto<NotificationListOutput>> GetBroadCastNotificationByUserIdAsync(NotificationListInput listInput);
    }
}