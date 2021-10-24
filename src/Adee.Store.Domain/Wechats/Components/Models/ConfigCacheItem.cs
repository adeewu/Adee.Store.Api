using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components.Models
{
    public class ConfigCacheItem
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// AES键
        /// </summary>
        public string EncodingAESKey { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
    }
}
