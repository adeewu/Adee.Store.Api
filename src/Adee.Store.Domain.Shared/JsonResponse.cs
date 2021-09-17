using System.Net;

namespace Adee.Store
{
    /// <summary>
    /// 响应类型
    /// </summary>
    public class JsonResponse
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// 响应类型
    /// </summary>
    public class JsonResponse<T> : JsonResponse
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }
    }
}
