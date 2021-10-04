using System;
using System.Net.Http;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 通知内容
    /// </summary>
    public class NotifyDto
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 地址参数
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// 提交内容，POST、PUT可用
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 触发域名
        /// </summary>
        public string TargetDomain { get; set; }
    }
}
