using System.Linq;

namespace Adee.Store.Pays
{
    public class DetailResponse<T>
    {
        /// <summary>
        /// 原始报文
        /// </summary>
        public string OriginResponse { get; set; }

        /// <summary>
        /// 解密报文
        /// </summary>
        public string EncryptResponse
        {
            get
            {
                if (Response == null) return string.Empty;

                return Response.ToJsonString();
            }
        }

        /// <summary>
        /// 原始报文
        /// </summary>
        public string OriginRequest { get; set; }

        /// <summary>
        /// 提交报文
        /// </summary>
        public string SubmitRequest { get; set; }

        /// <summary>
        /// 响应
        /// </summary>
        /// <value></value>
        public T Response { get; set; }
    }
}