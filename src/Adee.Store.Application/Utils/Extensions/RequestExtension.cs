using Adee.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class RequestExtension
    {
        /// <summary>
        /// 获取请求域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetDomain(this HttpRequest request)
        {
            if (request.Headers.ContainsKey("Origin"))
            {
                return request.Headers["Origin"].ToString();
            }
            if (request.Headers.ContainsKey("X-From-Where"))
            {
                return request.Headers["X-From-Where"];
            }

            var host = $"{request.Scheme}://{request.Host.Host}";
            if (request.Host.Port.HasValue)
            {
                host += ":" + request.Host.Port;
            }

            return host;
        }

        /// <summary>
        /// 获取Body内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<string> ReadBodyAsync(this HttpRequest request)
        {
            using (var sr = new StreamReader(request.Body))
            {
                return await sr.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 获取可序列化请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<Request> GetRequest(this HttpRequest request)
        {
            return new Request
            {
                Method = request.Method,
                Query = request.QueryString.HasValue ? request.QueryString.Value : string.Empty,
                Body = await request.ReadBodyAsync(),
                Headers = request.Headers.ToDictionary(p => p.Key, p => p.Value.ToArray()),
                Url = request.GetDisplayUrl(),
            };
        }
    }
}
