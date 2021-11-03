using Adee.Store.Attributes;
using Adee.Store.CallbackRequests;
using Adee.Store.Domain.Shared.Utils.Helpers;
using Adee.Store.Wechats.Components.Models;
using Adee.Store.Wechats.Components.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 微信第三方平台服务
    /// </summary>
    [ApiGroup(ApiGroupType.WechatComponent)]
    public class WechatComponentAppService : StoreWithRequestAppService
    {
        private readonly WechatComponentManager _wechatComponentManager;
        private readonly IRepository<CallbackRequest> _callbackRequestRepository;
        private readonly IRepository<WechatComponentAuth> _wechatComponentAuthRepository;
        private readonly SignHelper _signHelper;

        public WechatComponentAppService(
            WechatComponentManager wechatComponentManager,
            IRepository<CallbackRequest> callbackRequestRepository,
            IRepository<WechatComponentAuth> wechatComponentRepository,
            SignHelper signHelper
            )
        {
            _wechatComponentManager = wechatComponentManager;
            _callbackRequestRepository = callbackRequestRepository;
            _wechatComponentAuthRepository = wechatComponentRepository;
            _signHelper = signHelper;
        }

        /// <summary>
        /// 事件通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> AuthNotify([FromQuery] AuthNotifyDto dto)
        {
            var request = await Log(CallbackType.WechatComponentAuthNoity);

            try
            {
                var model = ObjectMapper.Map<AuthNotifyDto, Auth>(dto);

                await _wechatComponentManager.AuthNotify(model, request.Body);
                return "success";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"微信第三方平台事件通知处理失败，原因：{ex.Message}");

                return "fail";
            }
        }

        /// <summary>
        /// 获取授权地址（传递__tenantId）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>返回授权地址</returns>
        public async Task<string> AuthUrl(AuthUrlDto dto)
        {
            var model = ObjectMapper.Map<AuthUrlDto, AuthUrl>(dto);

            return await _wechatComponentManager.GetAuthUrl(model);
        }

        /// <summary>
        /// 授权成功(AuthUrl接口已传递__tenantId)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> AuthSuccess(AuthSuccessDto dto)
        {
            var query = await _wechatComponentManager.QueryAuth(dto.ComponentAppId, dto.auth_code);
            CheckHelper.IsNotNull(query, name: nameof(query));

            var authInfo = await _wechatComponentAuthRepository.FindAsync(p => p.AuthAppId == query.authorization_info.authorizer_appid && p.ComponentAppId == dto.ComponentAppId);
            var isUpdate = true;
            if (authInfo.IsNull())
            {
                authInfo = new WechatComponentAuth(GuidGenerator.Create());
                authInfo.TenantId = CurrentTenant.Id;
                isUpdate = false;
            }

            authInfo.AuthorizationCode = dto.auth_code;
            authInfo.AuthorizerRefreshToken = query.authorization_info.authorizer_refresh_token;
            authInfo.FuncInfo = query.authorization_info.func_info;

            if (isUpdate)
            {
                await _wechatComponentAuthRepository.UpdateAsync(authInfo);
            }
            else
            {
                await _wechatComponentAuthRepository.InsertAsync(authInfo);
            }

            return "授权完成";
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

        /// <summary>
        /// 第三方平台消息通知
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<string> GetNotify(string appId)
        {
            await Log(CallbackType.WechatComponentNotify, appId);

            return "success";
        }

        /// <summary>
        /// 第三方平台消息通知
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<string> PostNotify(string appId)
        {
            await Log(CallbackType.WechatComponentNotify, appId);

            return "success";
        }

        private async Task<Request> Log(CallbackType callbackType, string appId = default)
        {
            var request = await HttpContext.Request.GetRequest();
            if (appId.IsNullOrWhiteSpace() == false)
            {
                request.Headers.Add("AppId", new string[] { appId });
            }

            var callbackRequest = ObjectMapper.Map<Request, CallbackRequest>(request);
            callbackRequest.CallbackType = callbackType;
            callbackRequest.HashCode = _signHelper.Sign(new
            {
                callbackRequest.Url,
                callbackRequest.Method,
                callbackRequest.Query,
                callbackRequest.Body,
                callbackRequest.Header
            }, nameof(CallbackRequest), separator: "&", containKey: true);

            await _callbackRequestRepository.InsertAsync(callbackRequest, autoSave: true);

            return request;
        }
    }
}
