using Adee.Store.Pays.Utils.Helpers;
using Adee.Store.Wechats.Components.Clients;
using Adee.Store.Wechats.Components.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

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

        public WechatComponentManager(
            ILogger<WechatComponentManager> logger,
            IWechatComponentClient wechatComponentClient,
            IDistributedCache<ConfigCacheItem> configCache,
            IDistributedCache<AccessTokenCacheItem> accessTokenCache,
            IDistributedCache<ComponentVerifyTicketCacheItem> verifyTicketCache,
            XmlHelper xmlHelper,
            WechatCryptHelper wechatCryptHelper)
        {
            _logger = logger;
            _wechatComponentClient = wechatComponentClient;
            _configCache = configCache;
            _accessTokenCache = accessTokenCache;
            _verifyTicketCache = verifyTicketCache;
            _xmlHelper = xmlHelper;
            _wechatCryptHelper = wechatCryptHelper;
        }

        public async Task<string> GetAuthUrl(AuthUrl dto)
        {
            var accessTokenItem = await _accessTokenCache.GetAsync(dto.ComponentAppId);
            if (accessTokenItem == null || accessTokenItem.AccessToken.IsNullOrWhiteSpace())
            {
                var ticket = await _verifyTicketCache.GetAsync(dto.ComponentAppId);
                CheckHelper.IsNotNull(ticket, $"未找到ComponentVerifyTicket：{dto.ComponentAppId}的缓存");
                CheckHelper.IsNotNull(ticket.ComponentVerifyTicket, $"缓存出错，令牌不能为空");

                var configItem = await GetComponentConfig(dto.ComponentAppId);
                CheckHelper.IsNotNull(configItem.Secret, $"第三方平台：{dto.ComponentAppId}的密钥不能为空");

                var acceccTokenResult = await _wechatComponentClient.GetComponentAcceccToken(dto.ComponentAppId, configItem.Secret, ticket.ComponentVerifyTicket);
                CheckHelper.IsNotNull(acceccTokenResult, name: nameof(acceccTokenResult));
                CheckHelper.IsNotNull(acceccTokenResult.component_access_token, name: nameof(acceccTokenResult.component_access_token));
                CheckHelper.IsTrue(acceccTokenResult.expires_in > 0, "第三方平台令牌有效期不能小于120秒");

                if (accessTokenItem == null) accessTokenItem = new AccessTokenCacheItem();
                accessTokenItem.AccessToken = acceccTokenResult.component_access_token;
                accessTokenItem.ExpiresIn = acceccTokenResult.expires_in;

                await _accessTokenCache.SetAsync(dto.ComponentAppId, accessTokenItem, options: new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(acceccTokenResult.expires_in - 10)
                });
            }

            var preAuthCode = await _wechatComponentClient.GetPreAuthCode(dto.ComponentAppId, accessTokenItem.AccessToken);
            CheckHelper.IsNotNull(preAuthCode, name: nameof(preAuthCode));

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

            await Task.CompletedTask;
            _logger.LogCritical($"未知的通知类型：{authNotifyDto.InfoType}");
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
