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

    /// <summary>
    /// 使用授权码获取授权信息
    /// </summary>
    public class QueryAuthWechatResponse : WechatResponse
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthorizationInfo authorization_info { get; set; }
    }

    /// <summary>
    /// 授权信息
    /// </summary>
    public class AuthorizationInfo
    {
        /// <summary>
        /// string 授权方 appid
        /// </summary>
        public string authorizer_appid { get; set; }

        /// <summary>
        /// string 接口调用令牌（在授权的公众号/小程序具备 API 权限时，才有此返回值）
        /// </summary>
        public string authorizer_access_token { get; set; }

        /// <summary>
        /// number  authorizer_access_token 的有效期（在授权的公众号/小程序具备API权限时，才有此返回值），单位：秒
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// string 刷新令牌（在授权的公众号具备API权限时，才有此返回值），刷新令牌主要用于第三方平台获取和刷新已授权用户的 authorizer_access_token。一旦丢失，只能让用户重新授权，才能再次拿到新的刷新令牌。用户重新授权后，之前的刷新令牌会失效
        /// </summary>
        public string authorizer_refresh_token { get; set; }  

        /// <summary>
        /// 授权给开发者的权限集列表
        /// </summary>
        public string func_info { get; set; }     
    }

    /// <summary>
    /// 令牌信息
    /// </summary>
    public class AuthorizerTokenWechatReponse: WechatResponse
    {
        /// <summary>
        /// string 接口调用令牌（在授权的公众号/小程序具备 API 权限时，才有此返回值）
        /// </summary>
        public string authorizer_access_token { get; set; }

        /// <summary>
        /// number  authorizer_access_token 的有效期（在授权的公众号/小程序具备API权限时，才有此返回值），单位：秒
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// string 刷新令牌（在授权的公众号具备API权限时，才有此返回值），刷新令牌主要用于第三方平台获取和刷新已授权用户的 authorizer_access_token。一旦丢失，只能让用户重新授权，才能再次拿到新的刷新令牌。用户重新授权后，之前的刷新令牌会失效
        /// </summary>
        public string authorizer_refresh_token { get; set; }
    }
}