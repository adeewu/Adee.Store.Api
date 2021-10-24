using Adee.Store.Wechats.Components.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adee.Store.Wechats.Components
{
    public class AuthUrlDto
    {
        /// <summary>
        /// 第三方平台方 AppId
        /// </summary>
        /// <value></value>
        [Required]
        public string ComponentAppId { get; set; }

        /// <summary>
        /// 是否生成移动端地址
        /// </summary>
        /// <value></value>
        [Required]
        public bool IsMobile { get; set; }

        /// <summary>
        /// 回调 URI
        /// </summary>
        /// <value></value>
        [Required]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 要授权的帐号类型
        /// </summary>
        /// <value></value>
        public AuthType AuthType { get; set; } = AuthType.ALL;

        /// <summary>
        /// 指定授权唯一的小程序或公众号，AuthType、BizAppId两个字段互斥
        /// </summary>
        /// <value></value>
        public string BizAppId { get; set; }
    }
}
