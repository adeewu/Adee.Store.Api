using System;
using Volo.Abp.Application.Dtos;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class WechatComponentConfigDto : EntityDto<Guid>
    {
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

    /// <summary>
    /// 
    /// </summary>
    public class CreateUpdateWechatComponentConfigDto
    {
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

    /// <summary>
    /// 
    /// </summary>
    public class WechatComponentConfigListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 第三方平台AppId
        /// </summary>
        public string ComponentAppId { get; set; }

        /// <summary>
        ///禁用
        /// </summary>
        public bool? IsDisabled { get; set; }
    }
}
