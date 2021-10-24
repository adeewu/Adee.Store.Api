namespace Adee.Store.Wechats.Components.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WechatResponse
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }

    /// <summary>
    /// 第三方平台令牌响应
    /// </summary>
    public class ComponentTokenWechatResponse : WechatResponse
    {
        /// <summary>
        /// 第三方平台 access_token
        /// </summary>
        /// <value></value>
        public string component_access_token { get; set; }

        /// <summary>
        /// 有效期，单位：秒
        /// </summary>
        /// <value></value>
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 预授权码响应
    /// </summary>
    public class PreAuthCodeWechatResponse : WechatResponse
    {
        /// <summary>
        /// 预授权码
        /// </summary>
        /// <value></value>
        public string pre_auth_code { get; set; }

        /// <summary>
        /// 有效期，单位：秒
        /// </summary>
        /// <value></value>
        public string expires_in { get; set; }
    }
}