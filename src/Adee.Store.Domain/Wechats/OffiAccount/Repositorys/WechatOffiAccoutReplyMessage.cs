using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Adee.Store.Wechats.OffiAccount.Messages.Repositorys
{
    public class WechatOffiAccoutReplyMessage : AuditedAggregateRoot<Guid>, IIsDisabled
    {
        public WechatOffiAccoutReplyMessage(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 匹配类型
        /// </summary>
        public MatchType MatchType { get; set; }

        /// <summary>
        /// 被动回复消息类型
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 被动回复消息内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
