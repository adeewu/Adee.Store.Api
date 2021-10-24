using Adee.Store.Wechats.Components.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthNotifyDto
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 随机值
        /// </summary>
        public string nonce { get; set; }

        /// <summary>
        /// 加密类型
        /// </summary>
        public string encrypt_type { get; set; }

        /// <summary>
        /// 消息签名
        /// </summary>
        public string msg_signature { get; set; }
    }
}
