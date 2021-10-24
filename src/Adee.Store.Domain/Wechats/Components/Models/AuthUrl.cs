using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components.Models
{
    public class AuthUrl
    {
        /// <summary>
        /// 第三方平台方 AppId
        /// </summary>
        /// <value></value>
        public string ComponentAppId { get; set; }

        /// <summary>
        /// 是否生成移动端地址
        /// </summary>
        /// <value></value>
        public bool IsMobile { get; set; }

        /// <summary>
        /// 回调 URI
        /// </summary>
        /// <value></value>
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
