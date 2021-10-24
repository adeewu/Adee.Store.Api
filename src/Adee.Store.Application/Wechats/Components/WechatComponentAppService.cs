using Adee.Store.Attributes;
using Adee.Store.Wechats.Components.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 微信第三方平台服务
    /// </summary>
    [ApiGroup(ApiGroupType.WechatComponent)]
    public class WechatComponentAppService : StoreWithRequestAppService
    {
        private readonly WechatComponentManager _wechatComponentManager;

        public WechatComponentAppService(WechatComponentManager wechatComponentManager)
        {
            _wechatComponentManager = wechatComponentManager;
        }

        /// <summary>
        /// 事件通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> AuthNotify([FromQuery] AuthNotifyDto dto)
        {
            var body = await Request.ReadBodyAsync();
            Logger.LogDebug($"通知内容：{body}");

            var model = ObjectMapper.Map<AuthNotifyDto, Auth>(dto);

            try
            {
                await _wechatComponentManager.AuthNotify(model, body);
                return "success";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"微信第三方平台事件通知处理失败，原因：{ex.Message}");

                return "fail";
            }
        }

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>返回授权地址</returns>
        public async Task<string> AuthUrl(AuthUrlDto dto)
        {
            var model = ObjectMapper.Map<AuthUrlDto, AuthUrl>(dto);

            return await _wechatComponentManager.GetAuthUrl(model);
        }

        /// <summary>
        /// 开启ticket推送
        /// </summary>
        /// <param name="componentAppId"></param>
        /// <returns></returns>
        public async Task Open(string componentAppId)
        {
            await _wechatComponentManager.StartPushTicket(componentAppId);
        }

        public async Task<string> GetBusiness(string appId)
        {
            await Log();

            return "success";
        }

        public async Task<string> PostBusiness(string appId)
        {
            await Log();

            return "success";
        }

        private async Task Log()
        {
            var body = await Request.ReadBodyAsync();

            var query = string.Empty;
            if (Request.QueryString.HasValue)
            {
                query = Request.QueryString.Value;
            }

            Logger.LogWarning($"method：{Request.Method}，path：{Request.Path}，query：{query}，Body：{body}");
        }
    }
}
