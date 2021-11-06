using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Adee.Store.Wechats.Components.Repositorys
{
    /// <summary>
    /// 第三方平台配置
    /// </summary>
    public class WechatComponentConfig : AuditedAggregateRoot<Guid>
    {
        public WechatComponentConfig(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 第三方平台AppId
        /// </summary>
        public string ComponentAppId { get; set; }

        /// <summary>
        /// 消息校验Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 消息加解密Key
        /// </summary>
        public string EncodingAESKey { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        ///禁用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
