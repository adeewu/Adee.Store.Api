using Adee.Store.Attributes;
using Adee.Store.CallbackRequests;
using Adee.Store.Domain.Shared.Utils.Helpers;
using Adee.Store.Wechats.Components.Jobs.UpdateAccessToken;
using Adee.Store.Wechats.Components.Models;
using Adee.Store.Wechats.Components.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 微信第三方平台服务
    /// </summary>
    [ApiGroup(ApiGroupType.WechatComponent)]
    public class WechatComponentAppService : StoreWithRequestAppService
    {
        private readonly IWechatComponentManager _wechatComponentManager;
        private readonly IRepository<CallbackRequest> _callbackRequestRepository;
        private readonly IRepository<WechatComponentAuth> _wechatComponentAuthRepository;
        private readonly IRepository<WechatComponentConfig> _wechatComponentConfigRepository;
        private readonly SignHelper _signHelper;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IDistributedCache<UpdateAccessTokenArgs> _updateAccessTokenCache;
        private readonly IDistributedCache<UpdateComponentAccessTokenArgs> _updateComponentAccessTokenCache;

        public WechatComponentAppService(
            IWechatComponentManager wechatComponentManager,
            IRepository<CallbackRequest> callbackRequestRepository,
            IRepository<WechatComponentAuth> wechatComponentRepository,
            IRepository<WechatComponentConfig> wechatComponentConfigRepository,
            SignHelper signHelper,
            IBackgroundJobManager backgroundJobManager,
            IDistributedCache<UpdateAccessTokenArgs> updateAccessTokenCache,
            IDistributedCache<UpdateComponentAccessTokenArgs> updateComponentAccessTokenCache
            )
        {
            _wechatComponentManager = wechatComponentManager;
            _callbackRequestRepository = callbackRequestRepository;
            _wechatComponentAuthRepository = wechatComponentRepository;
            _wechatComponentConfigRepository = wechatComponentConfigRepository;
            _signHelper = signHelper;
            _backgroundJobManager = backgroundJobManager;
            _updateAccessTokenCache = updateAccessTokenCache;
            _updateComponentAccessTokenCache = updateComponentAccessTokenCache;
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
        /// 获取授权地址
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>返回授权地址</returns>
        public async Task<string> AuthUrl(AuthUrlDto dto)
        {
            var model = ObjectMapper.Map<AuthUrlDto, AuthUrl>(dto);

            return await _wechatComponentManager.GetAuthUrl(model, HttpContext.Request.GetDomain());
        }

        /// <summary>
        /// 授权成功
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetAuthSuccess()
        {
            var dto = new AuthSuccessDto
            {
                auth_code = HttpContext.Request.Query.Where(p => p.Key.ToLower() == "auth_code").Select(p => p.Value).FirstOrDefault(),
                expires_in = HttpContext.Request.Query.Where(p => p.Key.ToLower() == "expires_in").Select(p => Convert.ToInt32(p.Value)).FirstOrDefault(),
                Data = HttpContext.Request.Query.Where(p => p.Key.ToLower() == "data").Select(p => p.Value).FirstOrDefault()
            };

            var data = dto.Data.AsAnonymousObject(new { ComponentAppId = string.Empty, RedirectUrl = string.Empty, TenantId = string.Empty });

            Guid? tenantId = null;
            if (data.TenantId.IsNullOrWhiteSpace() == false)
            {
                tenantId = Guid.Parse(data.TenantId);
            }

            using (CurrentTenant.Change(tenantId))
            {
                var query = await _wechatComponentManager.QueryAuth(data.ComponentAppId, dto.auth_code);
                CheckHelper.IsNotNull(query, name: nameof(query));

                var authInfo = await _wechatComponentAuthRepository.FindAsync(p => p.AuthAppId == query.Authorization.AuthorizerAppId && p.ComponentAppId == data.ComponentAppId);
                var isUpdate = true;
                if (authInfo.IsNull())
                {
                    authInfo = new WechatComponentAuth(GuidGenerator.Create());
                    authInfo.TenantId = CurrentTenant.Id;
                    isUpdate = false;
                }

                authInfo.AuthorizationCode = dto.auth_code;
                authInfo.AuthorizerRefreshToken = query.Authorization.AuthorizerRefreshToken;
                authInfo.FuncInfo = query.Authorization.FunctionList.ToJsonString();
                authInfo.AuthAppId = query.Authorization.AuthorizerAppId;
                authInfo.ComponentAppId = data.ComponentAppId;
                authInfo.UnAuthorized = false;

                if (isUpdate)
                {
                    await _wechatComponentAuthRepository.UpdateAsync(authInfo);
                }
                else
                {
                    await _wechatComponentAuthRepository.InsertAsync(authInfo);
                }

                var args = new UpdateAccessTokenArgs
                {
                    AppId = query.Authorization.AuthorizerAppId,
                    UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    LastDelay = query.Authorization.ExpiresIn - WechatComponentConsts.ForwardUpdateAccessToken
                };
                await _updateAccessTokenCache.SetAsync(query.Authorization.AuthorizerAppId, args);
                await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromSeconds(args.LastDelay));

                if (data.RedirectUrl.IsNullOrWhiteSpace())
                {
                    var result = new ContentResult();
                    result.Content = "授权完成";

                    return result;
                }
                else
                {
                    return new RedirectResult(data.RedirectUrl);
                }

            }
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
        /// 重设令牌，适用第三方平台、授权App
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task ResetAccessToken(string appId)
        {
            var component = await _wechatComponentConfigRepository.SingleOrDefaultAsync(p => p.ComponentAppId == appId);
            if (component.IsNotNull())
            {
                CheckHelper.IsFalse(component.IsDisabled, $"第三方平台：{appId}已禁用");

                var accessToken = await _wechatComponentManager.UpdateComponentAccessToken(appId);

                var args = new UpdateComponentAccessTokenArgs
                {
                    ComponentAppId = appId,
                    LastDelay = accessToken.ExpiresIn - WechatComponentConsts.ForwardUpdateAccessToken,
                    UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                };
                await _updateComponentAccessTokenCache.SetAsync(appId, args);
                await _backgroundJobManager.EnqueueAsync(args);
            }

            var authApp = await _wechatComponentAuthRepository.SingleOrDefaultAsync(p => p.AuthAppId == appId);
            if (authApp.IsNotNull())
            {
                CheckHelper.IsFalse(authApp.UnAuthorized, $"AppId：{appId}已取消授权");

                var accessToken = await _wechatComponentManager.UpdateAccessToken(appId);

                var args = new UpdateAccessTokenArgs
                {
                    AppId = appId,
                    LastDelay = accessToken.ExpiresIn - WechatComponentConsts.ForwardUpdateAccessToken,
                    UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                };
                await _updateAccessTokenCache.SetAsync(appId, args);
                await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromSeconds(accessToken.ExpiresIn - WechatComponentConsts.ForwardUpdateAccessToken));
            }

            throw new UserFriendlyException($"AppId：{appId}无效");
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
                callbackRequest.Body,
                callbackRequest.Header
            }, nameof(CallbackRequest), separator: "&", containKey: true);

            var existHashCode = await _callbackRequestRepository.AnyAsync(p => p.HashCode == callbackRequest.HashCode);
            if (existHashCode == false)
            {
                await _callbackRequestRepository.InsertAsync(callbackRequest, autoSave: true);
            }

            return request;
        }
    }
}
