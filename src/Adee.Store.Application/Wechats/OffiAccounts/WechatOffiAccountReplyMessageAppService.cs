using Adee.Store.Attributes;
using Adee.Store.Wechats.OffiAccount;
using Adee.Store.Wechats.OffiAccount.Messages.Repositorys;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Wechats.OffiAccounts
{
    /// <summary>
    /// 被动回复消息
    /// </summary>
    [ApiGroup(ApiGroupType.WechatComponent)]
    public class WechatOffiAccountReplyMessageAppService : CrudAppService<WechatOffiAccoutReplyMessage, ReplyMessageDto, Guid, ReplyMessageListDto, CreateUpdateReplyMessageDto>
    {
        public WechatOffiAccountReplyMessageAppService(IRepository<WechatOffiAccoutReplyMessage, Guid> repository) : base(repository)
        {
        }
    }
}
