using System;
using Volo.Abp.Application.Dtos;

namespace Adee.Store.Wechats.OffiAccount
{
    public class ReplyMessageDto : EntityDto<Guid>
    {
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateUpdateReplyMessageDto
    {
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReplyMessageListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 匹配类型
        /// </summary>
        public MatchType? MatchType { get; set; }

        /// <summary>
        /// 被动回复消息类型
        /// </summary>
        public string MessageType { get; set; }
    }
}
