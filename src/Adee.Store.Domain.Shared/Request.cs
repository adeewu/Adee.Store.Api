using System.Collections.Generic;
using System.Linq;

namespace Adee.Store
{
    /// <summary>
    /// 请求
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 请求正文
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public Dictionary<string, string[]> Headers { get; set; }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
