namespace Adee.Store.Wechats.Components.Models
{
    /// <summary>
    /// 授权通知基类
    /// </summary>
    public class NotifyCacheItem
    {
        /// <summary>
        /// 第三方平台 appid
        /// </summary>
        /// <value></value>
        public string ComponentAppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <value></value>
        public long CreateTimestamp { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        /// <value></value>
        public string InfoType { get; set; }
    }

    /// <summary>
    /// 票据缓存
    /// </summary>
    public class ComponentVerifyTicketCacheItem : NotifyCacheItem
    {
        /// <summary>
        /// 票据
        /// </summary>
        /// <value></value>
        public string ComponentVerifyTicket { get; set; }

        /// <summary>
        /// 上一次票据
        /// </summary>
        /// <value></value>
        public string LastComponentVerifyTicket { get; set; }
    }

    /// <summary>
    /// 被动回复消息缓存
    /// </summary>
    public class ReplyMessageCacheItem
    {
        /// <summary>
        /// 回复关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; }
    }
}
