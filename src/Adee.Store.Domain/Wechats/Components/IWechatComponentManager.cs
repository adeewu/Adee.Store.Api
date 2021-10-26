using Adee.Store.Wechats.Components.Models;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 微信第三方平台服务
    /// </summary>
    public interface IWechatComponentManager
    {
        /// <summary>
        /// 授权通知
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task AuthNotify(Auth dto, string body);

        /// <summary>
        /// 启动票据推送服务
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <returns></returns>
        Task StartPushTicket(string componentAppId);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <param name="isMobile"></param>
        /// <returns></returns>
        Task<string> GetAuthUrl(AuthUrl dto);
    }
}
