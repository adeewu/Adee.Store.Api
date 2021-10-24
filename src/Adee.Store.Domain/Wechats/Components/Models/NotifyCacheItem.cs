using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string AppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <value></value>
        public long CreateTime { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        /// <value></value>
        public string InfoType { get; set; }
    }

    public class ComponentVerifyTicketCacheItem : NotifyCacheItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string ComponentVerifyTicket { get; set; }
    }

    public class AuthorizedCacheItem : NotifyCacheItem
    {
        /// <summary>
        /// 公众号或小程序的 appid
        /// </summary>
        /// <value></value>
        public string AuthorizerAppid { get; set; }

        /// <summary>
        /// 授权码，可用于获取授权信息
        /// </summary>
        /// <value></value>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// 授权码过期时间 单位秒
        /// </summary>
        /// <value></value>
        public long AuthorizationCodeExpiredTime { get; set; }

        /// <summary>
        /// 预授权码
        /// </summary>
        /// <value></value>
        public string PreAuthCode { get; set; }
    }
}
