using Adee.Store.Wechats.Components.Models;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components.Clients
{
    /// <summary>
    /// 微信第三方平台服务
    /// </summary>
    public interface IWechatComponentClient
    {
        /// <summary>
        /// 启动票据推送服务
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <param name="componentSecret"></param>
        /// <returns></returns>
        Task<WechatResponse> StartPushTicket(string componentAppId, string componentSecret);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <param name="componentSecret"></param>
        /// <param name="componentVerifyTicket"></param>
        /// <returns></returns>
        Task<ComponentTokenWechatResponse> GetComponentAcceccToken(string componentAppId, string componentSecret, string componentVerifyTicket);

        /// <summary>
        /// 获取预授权码
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <param name="componentAccessToken"></param>
        /// <returns></returns>
        Task<PreAuthCodeWechatResponse> GetPreAuthCode(string componentAppId, string componentAccessToken);

        /// <summary>
        /// 使用授权码获取授权信息
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <param name="componentAccessToken"></param>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        Task<QueryAuthWechatResponse> QueryAuth(string componentAppId, string componentAccessToken, string authorizationCode);

        /// <summary>
        /// 获取/刷新接口调用令牌
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <param name="componentAccessToken"></param>
        /// <param name="authorizerAppid"></param>
        /// <param name="authorizerRefreshToken"></param>
        /// <returns></returns>
        Task<AuthorizerTokenWechatReponse> AuthorizerToken(string componentAppId, string componentAccessToken, string authorizerAppid, string authorizerRefreshToken);
    }
}
