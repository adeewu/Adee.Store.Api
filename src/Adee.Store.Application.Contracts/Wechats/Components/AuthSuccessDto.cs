using Adee.Store.Wechats.Components.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 授权成功
    /// </summary>
    public class AuthSuccessDto
    {
        /// <summary>
        /// 授权码
        /// </summary>
        [Required]
        public string auth_code { get; set; }

        /// <summary>
        /// 过期时间，秒
        /// </summary>
        [Required]
        public int expires_in { get; set; }

        /// <summary>
        /// 第三方平台Id
        /// </summary>
        [Required]
        public string ComponentAppId { get; set; }
    }
}
