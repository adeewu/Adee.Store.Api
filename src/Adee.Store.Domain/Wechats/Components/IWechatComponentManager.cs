using Adee.Store.Wechats.Components.Models;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 微信第三方平台服务
    /// </summary>
    public interface IWechatComponentManager
    {
        /// <summary>
        /// 更新第三方平台令牌
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <returns></returns>
        Task<AccessTokenCacheItem> UpdateComponentAccessToken(string componentAppId);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        Task<AccessTokenCacheItem> GetAccessToken(string appId);

        /// <summary>
        /// 更新令牌
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        Task<AccessTokenCacheItem> UpdateAccessToken(string appId);

        /// <summary>
        /// 使用授权码获取授权信息
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        Task<CgibinComponentApiQueryAuthResponse> QueryAuth(string componentAppId, string authorizationCode);

        /// <summary>
        /// 授权通知
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task AuthNotify(Auth auth, string body);

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="auth"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<string> MessageNotify(string appId, Auth auth, string body);

        /// <summary>
        /// 启动票据推送服务
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <returns></returns>
        Task StartPushTicket(string componentAppId);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="AuthUrl"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        Task<string> GetAuthUrl(AuthUrl dto, string domain);
    }
}
