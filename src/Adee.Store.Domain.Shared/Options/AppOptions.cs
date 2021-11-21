using System;
using System.Linq;

namespace Adee.Store
{
    /// <summary>
    /// App设置
    /// </summary>
    public class AppOptions
    {
        /// <summary>
        /// 托管地址
        /// </summary>
        public string SelfUrl { get; set; }

        /// <summary>
        /// 跨域域名
        /// </summary>
        public string CorsOrigins { get; set; }

        /// <summary>
        /// 跨域域名列表
        /// </summary>
        public string[] CorsOriginArray
        {
            get
            {
                if (CorsOrigins.IsNullOrWhiteSpace()) return new string[0];

                return CorsOrigins.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
            }
        }
    }
}