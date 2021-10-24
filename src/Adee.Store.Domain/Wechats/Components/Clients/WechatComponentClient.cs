using Adee.Store.Pays.Utils.Helpers;
using Adee.Store.Wechats.Components.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Wechats.Components.Clients
{
    public class WechatComponentClient : IWechatComponentClient, ITransientDependency
    {
        private readonly ICommonClient _commonClient;

        public WechatComponentClient(
            ICommonClient commonClient
            )
        {
            _commonClient = commonClient;
            _commonClient.SetBaseAddress("https://api.weixin.qq.com/cgi-bin/component");
        }

        public async Task<ComponentTokenWechatResponse> GetComponentAcceccToken(string componentAppId, string componentSecret, string componentVerifyTicket)
        {
            return await PostAsync<ComponentTokenWechatResponse>("/api_component_token", body: new
            {
                component_appid = componentAppId,
                component_appsecret = componentSecret,
                component_verify_ticket = componentVerifyTicket,
            });
        }

        public async Task<PreAuthCodeWechatResponse> GetPreAuthCode(string componentAppId, string componentAccessToken)
        {
            return await PostAsync<PreAuthCodeWechatResponse>("/api_create_preauthcode", query: new
            {
                component_access_token = componentAccessToken,
            }, body: new
            {
                component_appid = componentAppId,
            });
        }

        public async Task<WechatResponse> StartPushTicket(string componentAppId, string componentSecret)
        {
            return await PostAsync<WechatResponse>("/api_start_push_ticket", query: new
            {
                component_appid = componentAppId,
                component_secret = componentSecret
            });
        }

        private async Task<T> PostAsync<T>(string url, object query = null, object body = null) where T : WechatResponse, new()
        {
            var result = await _commonClient.PostAsync<T>(url, query, body);

            if (result.errcode != null)
            {
                CheckHelper.AreEqual(result.errcode, "0", message: $"请求异常，返回码：{result.errcode}，消息：{result.errmsg}");
            }

            return result;
        }
    }
}
