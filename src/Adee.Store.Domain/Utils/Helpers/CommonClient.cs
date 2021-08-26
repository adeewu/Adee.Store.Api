using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Pays.Utils.Helpers
{
    public class CommonClient : ICommonClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CommonClient> _logger;

        public CommonClient(HttpClient httpClient, ILogger<CommonClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void SetBaseAddress(string url)
        {
            SetBaseAddress(new Uri(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public void SetBaseAddress(Uri uri)
        {
            _httpClient.BaseAddress = uri;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<string> PostStringAsync(string url, object query = null, object body = null, object header = null, Encoding encoding = null)
        {
            return await SendAsync(HttpMethod.Post, url, query: query, body: body, header: header, encoding: encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, object query = null, object body = null, object header = null, Encoding encoding = null) where T : class, new()
        {
            var result = await PostStringAsync(url, query: query, body: body, header: header, encoding: encoding);

            return result.AsObject<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string url, object query = null)
        {
            return await SendAsync(HttpMethod.Get, url, query: query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url, object query = null) where T : class, new()
        {
            var result = await GetStringAsync(url, query);

            return result.AsObject<T>();
        }

        /// <summary>
        /// 通用请求
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="body">post、put才有用</param>
        /// <param name="encoding">post、put才有用</param>
        /// <returns></returns>
        public async Task<string> SendAsync(HttpMethod httpMethod, string url, object query = null, object body = null, object header = null, Encoding encoding = null)
        {
            var querys = new Dictionary<string, string>();
            if (query != null)
            {
                if (query is Dictionary<string, string>)
                {
                    querys = query as Dictionary<string, string>;
                }
                else
                {
                    querys = query.GetType().GetRuntimeProperties().ToDictionary(p => p.Name, p => p.GetValue(query) == null ? string.Empty : p.GetValue(query).ToString());
                }
            }

            var headers = new Dictionary<string, string>();
            if (header != null)
            {
                if (header is Dictionary<string, string>)
                {
                    headers = header as Dictionary<string, string>;
                }
                else
                {
                    headers = header.GetType().GetRuntimeProperties().ToDictionary(p => p.Name, p => p.GetValue(header) == null ? string.Empty : p.GetValue(header).ToString());
                }
            }

            return await SendAsync(httpMethod, url, query: querys, body: body, header: headers, encoding: encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="body"></param>
        /// <param name="header"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<string> SendAsync(HttpMethod httpMethod, string url, Dictionary<string, string> query = null, object body = null, Dictionary<string, string> header = null, Encoding encoding = null)
        {
            EventId eventId = GetHashCode();

            var response = await GetResponseMessage(eventId, httpMethod, url, query, body, header, encoding);

            var result = await response.Content.ReadAsStringAsync();
            _logger.LogDebug(eventId, "响应内容：" + result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filename"></param>
        /// <param name="httpMethod"></param>
        /// <param name="query"></param>
        /// <param name="body"></param>
        /// <param name="header"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task DownloadFileAsync(string url, string filename, HttpMethod httpMethod = null, Dictionary<string, string> query = null, object body = null, Dictionary<string, string> header = null, Encoding encoding = null)
        {
            EventId eventId = GetHashCode();

            var response = await GetResponseMessage(eventId, httpMethod, url, query, body, header, encoding);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            var stream = await response.Content.ReadAsStreamAsync();
            using (var fs = new FileStream(filename, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    var size = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (size <= 0) break;

                    await fs.WriteAsync(buffer, 0, size);
                }
            }
        }

        private async Task<HttpResponseMessage> GetResponseMessage(EventId eventId, HttpMethod httpMethod, string url, Dictionary<string, string> query = null, object body = null, Dictionary<string, string> header = null, Encoding encoding = null)
        {
            if (httpMethod == null) httpMethod = HttpMethod.Get;
            if (query == null) query = new Dictionary<string, string>();
            if (header == null) header = new Dictionary<string, string>();
            if (encoding == null) encoding = Encoding.UTF8;

            _logger.LogDebug(eventId, $"请求方式：{httpMethod}");

            var requestUrl = _httpClient.GetUrl(url, query);
            _logger.LogDebug(eventId, $"请求url：{requestUrl}");

            var requestMessage = new HttpRequestMessage(httpMethod, requestUrl);

            if (header != null)
            {
                header.ForEach(p => requestMessage.Headers.TryAddWithoutValidation(p.Key, p.Value));
            }

            if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && body != null)
            {
                var content = body.ToJsonString();
                _logger.LogDebug(eventId, $"提交内容：{content}");

                var contentType = header.Where(p => p.Key.ToLower() == "content-type").Select(p => p.Value.ToLower()).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(contentType)) contentType = "application/json";

                if (contentType == "application/json")
                {
                    requestMessage.Content = new StringContent(content, encoding, contentType);
                }

                if (contentType == "application/x-www-form-urlencoded")
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.FromObject(body) as IEnumerable<KeyValuePair<string, Newtonsoft.Json.Linq.JToken>>;
                    requestMessage.Content = new FormUrlEncodedContent(jObject.Select(jt => new KeyValuePair<string, string>(jt.Key, (string)jt.Value)));
                }

                if (contentType == "multipart/form-data")
                {
                    requestMessage.Content = new MultipartFormDataContent(body.ToString());
                }

                CheckHelper.IsNotNull(requestMessage.Content, $"暂不支持Content-Type：{contentType}");
            }

            var response = await _httpClient.SendAsync(requestMessage);
            CheckHelper.IsTrue(response.IsSuccessStatusCode, $"请求发生错误，请求方式：{httpMethod}，返回码：{response.StatusCode}");

            return response;
        }
    }
}
