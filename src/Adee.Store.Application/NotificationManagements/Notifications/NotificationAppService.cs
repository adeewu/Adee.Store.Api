using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adee.Store.Attributes;
using Adee.Store.NotificationManagement.Hubs;
using Adee.Store.NotificationManagement.Notifications.Dtos;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Adee.Store.NotificationManagement.Notifications
{
    /// <summary>
    /// SignalR消息通知
    /// </summary>
    [ApiGroup(ApiGroupType.Notification)]
    public class NotificationAppService : StoreAppService, INotificationAppService
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly NotificationManager _notificationManager;
        private readonly ICurrentUser _currentUser;
        private readonly IDapperNotificationRepository _dapperNotificationRepository;
        public NotificationAppService(
            IHubContext<NotificationHub, INotificationHub> hubContext,
            NotificationManager notificationManager,
            ICurrentUser currentUser,
            IDapperNotificationRepository dapperNotificationRepository)
        {
            _hubContext = hubContext;
            _notificationManager = notificationManager;
            _currentUser = currentUser;
            _dapperNotificationRepository = dapperNotificationRepository;
        }

        /// <summary>
        /// 标记已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task MarkReadAsync(MarkReadInput input)
        {
            return _notificationManager.MarkReadAsync(input.Id, input.ReceiveId);
        }

        /// <summary>
        /// 新增消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateAsync(CreateNotificationInput input)
        {
            if (_currentUser.IsAuthenticated)
            {
                await _notificationManager.CreateAsync(input.Title, input.Content, _currentUser.Id.Value, input.ReceiveIds, input.MessageType);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public async Task SendMessageAsync(string title, string content, MessageType messageType, List<string> users)
        {
            switch (messageType)
            {
                case MessageType.Text:
                    await SendMessageToClientByUserIdAsync(new SendNotificationDto(title, content, messageType), users);
                    break;
                case MessageType.BroadCast:
                    await SendMessageToAllClientAsync(new SendNotificationDto(title, content, messageType));
                    break;
                default:
                    throw new UserFriendlyException("未知的消息类型");
            }
        }

        /// <summary>
        /// 分页获取用户普通文本消息
        /// </summary>
        /// <param name="listInput"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<NotificationListOutput>> GetTextNotificationByUserIdAsync(NotificationListInput listInput)
        {
            if (!_currentUser.Id.HasValue)
            {
                return null;
            }

            var totalCount = await _dapperNotificationRepository.GetPageTextNotificationCountByUserIdAsync(_currentUser.Id.Value);

            var list = await _dapperNotificationRepository.GetTextNotificationByUserIdAsync(_currentUser.Id.Value, listInput.PageSize, listInput.GetSkipCount());

            return new PagedResultDto<NotificationListOutput>(totalCount, list);
        }

        /// <summary>
        /// 分页获取广播消息
        /// </summary>
        /// <param name="listInput"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<NotificationListOutput>> GetBroadCastNotificationByUserIdAsync(NotificationListInput listInput)
        {
            if (!_currentUser.Id.HasValue)
            {
                return null;
            }

            var totalCount = await _dapperNotificationRepository.GetPageBroadCastNotificationCountByUserIdAsync(_currentUser.Id.Value);

            var list = await _dapperNotificationRepository.GetBroadCastNotificationByUserIdAsync(_currentUser.Id.Value, listInput.PageSize, listInput.GetSkipCount());

            return new PagedResultDto<NotificationListOutput>(totalCount, list);
        }

        /// <summary>
        /// 发送消息指定客户端用户
        /// </summary>
        /// <param name="sendNotificationDto"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        private async Task SendMessageToClientByUserIdAsync(SendNotificationDto sendNotificationDto, List<string> users)
        {
            if (users is { Count: > 0 })
            {
                await _hubContext.Clients
                    .Users(users.AsReadOnly().ToList())
                    .ReceiveTextMessageAsync(sendNotificationDto);
            }
        }

        /// <summary>
        /// 发送消息到所有客户端
        /// 广播消息
        /// </summary>
        /// <param name="sendNotificationDto"></param>
        private async Task SendMessageToAllClientAsync(SendNotificationDto sendNotificationDto)
        {
            await _hubContext.Clients.All.ReceiveBroadCastMessageAsync(sendNotificationDto);
        }
    }
}