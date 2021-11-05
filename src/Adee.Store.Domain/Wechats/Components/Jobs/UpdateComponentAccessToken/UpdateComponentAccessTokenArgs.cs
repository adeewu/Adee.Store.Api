using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components.Jobs.UpdateAccessToken
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateComponentAccessTokenArgs
    {
        /// <summary>
        /// 第三方平台AppId
        /// </summary>
        public string ComponentAppId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }

        /// <summary>
        /// 上次等待时间
        /// </summary>
        public int LastDelay { get; set; } = 7000;
    }
}
