using Adee.Store.Pays.Utils.Helpers;
using Adee.Store.Wechats.Components.Clients;
using Adee.Store.Wechats.Components.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Events;
using System;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Wechats.Components
{
    public class WechatComponentManager : IWechatComponentManager, ITransientDependency
    {
        private readonly ILogger<WechatComponentManager> _logger;
        private readonly IWechatComponentClient _wechatComponentClient;
        private readonly IDistributedCache<ConfigCacheItem> _configCache;
        private readonly IDistributedCache<AccessTokenCacheItem> _accessTokenCache;
        private readonly IDistributedCache<ComponentVerifyTicketCacheItem> _verifyTicketCache;
        private readonly XmlHelper _xmlHelper;
        private readonly WechatCryptHelper _wechatCryptHelper;
        private readonly ICurrentTenant _currentTenant;

        public WechatComponentManager(
            ILogger<WechatComponentManager> logger,
            IWechatComponentClient wechatComponentClient,
            IDistributedCache<ConfigCacheItem> configCache,
            IDistributedCache<AccessTokenCacheItem> accessTokenCache,
            IDistributedCache<ComponentVerifyTicketCacheItem> verifyTicketCache,
            XmlHelper xmlHelper,
            WechatCryptHelper wechatCryptHelper,
            ICurrentTenant currentTenant)
        {
            _logger = logger;
            _wechatComponentClient = wechatComponentClient;
            _configCache = configCache;
            _accessTokenCache = accessTokenCache;
            _verifyTicketCache = verifyTicketCache;
            _xmlHelper = xmlHelper;
            _wechatCryptHelper = wechatCryptHelper;
            _currentTenant = currentTenant;
        }

        public async Task<AccessTokenCacheItem> GetComponentAccessToken(string componentAppId)
        {
            var accessTokenItem = await _accessTokenCache.GetAsync(componentAppId);
            if (accessTokenItem == null || accessTokenItem.AccessToken.IsNullOrWhiteSpace())
            {
                var ticket = await _verifyTicketCache.GetAsync(componentAppId);
                CheckHelper.IsNotNull(ticket, $"未找到ComponentVerifyTicket：{componentAppId}的缓存");
                CheckHelper.IsNotNull(ticket.ComponentVerifyTicket, $"缓存出错，令牌不能为空");

                var configItem = await GetComponentConfig(componentAppId);
                CheckHelper.IsNotNull(configItem.Secret, $"第三方平台：{componentAppId}的密钥不能为空");

                var acceccTokenResult = await _wechatComponentClient.GetComponentAcceccToken(componentAppId, configItem.Secret, ticket.ComponentVerifyTicket);
                CheckHelper.IsNotNull(acceccTokenResult, name: nameof(acceccTokenResult));
                CheckHelper.IsNotNull(acceccTokenResult.component_access_token, name: nameof(acceccTokenResult.component_access_token));
                CheckHelper.IsTrue(acceccTokenResult.expires_in > 0, "第三方平台令牌有效期不能小于120秒");

                if (accessTokenItem == null) accessTokenItem = new AccessTokenCacheItem();
                accessTokenItem.AccessToken = acceccTokenResult.component_access_token;
                accessTokenItem.ExpiresIn = acceccTokenResult.expires_in;

                await _accessTokenCache.SetAsync(componentAppId, accessTokenItem, options: new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(acceccTokenResult.expires_in - 10)
                });
            }

            return accessTokenItem;
        }

        public async Task<AccessTokenCacheItem> GetAccessToken(string appId, string componentAppId)
        {
            var accessTokenItem = await _accessTokenCache.GetAsync(appId);
            if (accessTokenItem == null || accessTokenItem.AccessToken.IsNullOrWhiteSpace())
            {

                var componentAccessToken = await GetComponentAccessToken(componentAppId);

                var appAccessToken = await _wechatComponentClient.AuthorizerToken(componentAppId, componentAccessToken.AccessToken, appId, "???");

                if (accessTokenItem == null) accessTokenItem = new AccessTokenCacheItem();
                accessTokenItem.AccessToken = appAccessToken.authorizer_access_token;
                accessTokenItem.ExpiresIn = appAccessToken.expires_in;

                await _accessTokenCache.SetAsync(appId, accessTokenItem, options: new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(appAccessToken.expires_in - 10)
                });
            }

            return accessTokenItem;
        }

        public async Task<string> GetAuthUrl(AuthUrl dto)
        {
            var accessTokenItem = await GetComponentAccessToken(dto.ComponentAppId);

            var preAuthCode = await _wechatComponentClient.GetPreAuthCode(dto.ComponentAppId, accessTokenItem.AccessToken);
            CheckHelper.IsNotNull(preAuthCode, name: nameof(preAuthCode));

            dto.RedirectUrl = (dto.RedirectUrl.Contains('?') ? "&" : "?") + $"__tenantId={_currentTenant.Id}";
            dto.RedirectUrl += $"&ComponentAppId={dto.ComponentAppId}";
            dto.RedirectUrl = HttpUtility.UrlEncode(dto.RedirectUrl);

            if (dto.IsMobile && string.IsNullOrWhiteSpace(dto.BizAppId) == true)
            {
                return $"https://mp.weixin.qq.com/safe/bindcomponent?action=bindcomponent&no_scan=1&component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.pre_auth_code}&redirect_uri={dto.RedirectUrl}&auth_type={(int)dto.AuthType}#wechat_redirect";
            }
            if (dto.IsMobile && string.IsNullOrWhiteSpace(dto.BizAppId) == false)
            {
                return $"https://mp.weixin.qq.com/safe/bindcomponent?action=bindcomponent&no_scan=1&component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.pre_auth_code}&redirect_uri={dto.RedirectUrl}&biz_appid={dto.BizAppId}#wechat_redirect";
            }
            return $"https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.pre_auth_code}&redirect_uri={dto.RedirectUrl}&auth_type={(int)dto.AuthType}";
        }

        public async Task StartPushTicket(string componentAppId)
        {
            var configItem = await GetComponentConfig(componentAppId);
            CheckHelper.IsNotNull(configItem.Secret, $"第三方平台：{componentAppId}的密钥不能为空");

            await _wechatComponentClient.StartPushTicket(componentAppId, configItem.Secret);
        }

        public async Task AuthNotify(Auth dto, string body)
        {
            CheckHelper.IsNotNull(body, name: nameof(body));

            var encryptDto = _xmlHelper.Deserialize<AuthEncrypt>(body);
            CheckHelper.IsNotNull(encryptDto, $"解密失败，内容：{body}");

            var componentConfigDto = await GetComponentConfig(encryptDto.AppId);
            CheckHelper.IsNotNull(componentConfigDto, $"获取");

            _wechatCryptHelper.Init(componentConfigDto.Token, componentConfigDto.EncodingAESKey, componentConfigDto.AppId);
            var encryptString = _wechatCryptHelper.DecryptMsg(dto.msg_signature, dto.timestamp, dto.nonce, encryptDto.Encrypt);
            _logger.LogDebug($"解密内容：{encryptString}");

            var authNotifyDto = _xmlHelper.Deserialize<NotifyCacheItem>(encryptString);

            if (authNotifyDto.InfoType == "component_verify_ticket")
            {
                var ticket = _xmlHelper.Deserialize<ComponentVerifyTicketCacheItem>(encryptString);

                var expireTime = DateTimeOffset.FromUnixTimeSeconds(ticket.CreateTime).AddHours(12);

                await _verifyTicketCache.SetAsync(ticket.AppId, ticket, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = expireTime,
                });
                return;
            }

            if (authNotifyDto.InfoType == "unauthorized")
            {

            }

            if (authNotifyDto.InfoType == "updateauthorized")
            {

            }

            if (authNotifyDto.InfoType == "authorized")
            {

            }

            _logger.LogCritical($"未知的通知类型：{authNotifyDto.InfoType}");
        }

        public async Task<QueryAuthWechatResponse> QueryAuth(string componentAppId, string authorizationCode)
        {
            var accessTokenItem = await GetComponentAccessToken(componentAppId);

            var result = await _wechatComponentClient.QueryAuth(componentAppId, accessTokenItem.AccessToken, authorizationCode);
            await _accessTokenCache.SetAsync(result.authorization_info.authorizer_appid, new AccessTokenCacheItem
            {
                AccessToken = result.authorization_info.authorizer_access_token,
                ExpiresIn = result.authorization_info.expires_in,
            });

            return result;
        }

        private async Task<ConfigCacheItem> GetComponentConfig(string componentAppId)
        {
            var dto = await _configCache.GetAsync(componentAppId);
            if (dto.IsNotNull()) return dto;

            dto = new ConfigCacheItem
            {
                AppId = componentAppId,
                Token = "adee",
                EncodingAESKey = "adee123456adee123456adee123456adee123456789",
                Secret = "8cb872325aac675becd1e308324271f0",
            };
            await _configCache.SetAsync(componentAppId, dto);

            return dto;
        }
    }
}
