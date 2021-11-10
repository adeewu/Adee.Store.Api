using Adee.Store.Wechats.Components.Jobs.UpdateAccessToken;
using Adee.Store.Wechats.Components.Models;
using Adee.Store.Wechats.Components.Repositorys;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Events;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Adee.Store.Wechats.Components
{
    public class WechatComponentManager : IWechatComponentManager, ITransientDependency
    {
        private readonly ILogger<WechatComponentManager> _logger;
        private readonly IDistributedCache<ComponentConfigCacheItem> _configCache;
        private readonly IDistributedCache<AccessTokenCacheItem> _accessTokenCache;
        private readonly IDistributedCache<ComponentVerifyTicketCacheItem> _verifyTicketCache;
        private readonly IDistributedCache<UpdateAccessTokenArgs> _updateAccessTokenCache;
        private readonly IRepository<WechatComponentConfig> _wechatComponentConfigRespository;
        private readonly IRepository<WechatComponentAuth> _wechatComponentAuthRespository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IObjectMapper _objectMapper;

        public WechatComponentManager(
            ILogger<WechatComponentManager> logger,
            IDistributedCache<ComponentConfigCacheItem> configCache,
            IDistributedCache<AccessTokenCacheItem> accessTokenCache,
            IDistributedCache<ComponentVerifyTicketCacheItem> verifyTicketCache,
            IDistributedCache<UpdateAccessTokenArgs> updateAccessTokenCache,
            IRepository<WechatComponentConfig> wechatComponentConfigRespository,
            IRepository<WechatComponentAuth> wechatComponentAuthRespository,
            ICurrentTenant currentTenant,
            IObjectMapper objectMapper)
        {
            _logger = logger;
            _configCache = configCache;
            _accessTokenCache = accessTokenCache;
            _verifyTicketCache = verifyTicketCache;
            _updateAccessTokenCache = updateAccessTokenCache;
            _wechatComponentConfigRespository = wechatComponentConfigRespository;
            _wechatComponentAuthRespository = wechatComponentAuthRespository;
            _currentTenant = currentTenant;
            _objectMapper = objectMapper;
        }

        public async Task<AccessTokenCacheItem> UpdateComponentAccessToken(string componentAppId)
        {
            var ticket = await _verifyTicketCache.GetAsync(componentAppId);
            CheckHelper.IsNotNull(ticket, $"未找到ComponentVerifyTicket：{componentAppId}的缓存");
            CheckHelper.IsNotNull(ticket.ComponentVerifyTicket, $"缓存出错，令牌不能为空");

            var client = await GetComponentClient(componentAppId);
            var acceccTokenResult = await client.ExecuteCgibinComponentApiComponentTokenAsync(new CgibinComponentApiComponentTokenRequest
            {
                ComponentAppId = client.Credentials.AppId,
                ComponentSecret = client.Credentials.AppSecret,
                ComponentVerifyTicket = ticket.ComponentVerifyTicket,
            });
            ProcessResult(acceccTokenResult);

            CheckHelper.IsNotNull(acceccTokenResult, name: nameof(acceccTokenResult));
            CheckHelper.IsNotNull(acceccTokenResult.ComponentAccessToken, name: nameof(acceccTokenResult.ComponentAccessToken));
            CheckHelper.IsTrue(acceccTokenResult.ExpiresIn > 0, "第三方平台令牌有效期不能小于0秒");

            var accessTokenItem = new AccessTokenCacheItem
            {
                AccessToken = acceccTokenResult.ComponentAccessToken,
                ExpiresIn = acceccTokenResult.ExpiresIn,
            };

            await _accessTokenCache.SetAsync(componentAppId, accessTokenItem, options: new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(acceccTokenResult.ExpiresIn)
            });

            return accessTokenItem;
        }

        public async Task<AccessTokenCacheItem> GetAccessToken(string appId)
        {
            var accessTokenItem = await _accessTokenCache.GetAsync(appId);
            if(accessTokenItem.IsNotNull() && accessTokenItem.AccessToken.IsNotNull())
            {
                return accessTokenItem;
            }

            var auth = await _wechatComponentAuthRespository.GetAsync(p => p.AuthAppId == appId);
            CheckHelper.IsNotNull(auth, name: nameof(auth));

            var response = await QueryAuth(auth.ComponentAppId, auth.AuthorizationCode);
            CheckHelper.IsNotNull(response, name: nameof(response));

            auth.AuthorizerRefreshToken = response.Authorization.AuthorizerRefreshToken;
            await _wechatComponentAuthRespository.UpdateAsync(auth, autoSave: true);

            accessTokenItem = new AccessTokenCacheItem
            {
                AccessToken = response.Authorization.AuthorizerAccessToken,
                ExpiresIn = response.Authorization.ExpiresIn,
                RefreshToken = response.Authorization.AuthorizerRefreshToken,
            };


            return accessTokenItem;
        }

        public async Task<AccessTokenCacheItem> UpdateAccessToken(string appId)
        {
            var auth = await _wechatComponentAuthRespository.GetAsync(p => p.AuthAppId == appId);
            CheckHelper.IsNotNull(auth, $"AppId：{appId}授权信息不存在");
            CheckHelper.IsNotNull(auth.ComponentAppId, $"AppId：{appId}的授权第三方平台信息错误");

            var componentAccessToken = await GetAccessToken(auth.ComponentAppId);
            CheckHelper.IsNotNull(componentAccessToken, name: nameof(componentAccessToken));

            var client = await GetComponentClient(auth.ComponentAppId);
            var appAccessToken = await client.ExecuteCgibinComponentApiAuthorizerTokenAsync(new CgibinComponentApiAuthorizerTokenRequest
            {
                AuthorizerAppId = appId,
                ComponentAppId = auth.ComponentAppId,
                ComponentAccessToken = componentAccessToken.AccessToken,
                AuthorizerRefreshToken = auth.AuthorizerRefreshToken,
            });

            var accessTokenItem = new AccessTokenCacheItem
            {
                AccessToken = appAccessToken.AuthorizerAccessToken,
                ExpiresIn = appAccessToken.ExpiresIn,
                RefreshToken = appAccessToken.AuthorizerRefreshToken,
            };

            await _accessTokenCache.SetAsync(appId, accessTokenItem, options: new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(appAccessToken.ExpiresIn)
            });

            return accessTokenItem;
        }

        public async Task<string> GetAuthUrl(AuthUrl dto, string domain)
        {
            var accessTokenItem = await GetAccessToken(dto.ComponentAppId);

            var client = await GetComponentClient(dto.ComponentAppId);
            var preAuthCode = await client.ExecuteCgibinComponentApiCreatePreAuthCodeAsync(new CgibinComponentApiCreatePreAuthCodeRequest
            {
                ComponentAccessToken = accessTokenItem.AccessToken,
                ComponentAppId = dto.ComponentAppId,
            });
            CheckHelper.IsNotNull(preAuthCode, name: nameof(preAuthCode));

            var data = new
            {
                dto.RedirectUrl,
                dto.ComponentAppId,
                TenantId = _currentTenant.Id
            }.ToJsonString();
            var url = $"{domain}/api/store/wechat-component/auth-success?Data={HttpUtility.UrlEncode(data)}";
            url = HttpUtility.UrlEncode(url);

            if (dto.IsMobile && string.IsNullOrWhiteSpace(dto.BizAppId) == true)
            {
                return $"https://mp.weixin.qq.com/safe/bindcomponent?action=bindcomponent&no_scan=1&component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.PreAuthCode}&redirect_uri={url}&auth_type={(int)dto.AuthType}#wechat_redirect";
            }
            if (dto.IsMobile && string.IsNullOrWhiteSpace(dto.BizAppId) == false)
            {
                return $"https://mp.weixin.qq.com/safe/bindcomponent?action=bindcomponent&no_scan=1&component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.PreAuthCode}&redirect_uri={url}&biz_appid={dto.BizAppId}#wechat_redirect";
            }
            return $"https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.PreAuthCode}&redirect_uri={url}&auth_type={(int)dto.AuthType}";
        }

        public async Task StartPushTicket(string componentAppId)
        {
            var configItem = await GetComponentConfig(componentAppId);
            CheckHelper.IsNotNull(configItem.Secret, $"第三方平台：{componentAppId}的密钥不能为空");

            var client = await GetComponentClient(componentAppId);
            var result = await client.ExecuteCgibinComponentApiStartPushTicketAsync(new CgibinComponentApiStartPushTicketRequest
            {
                ComponentAppId = componentAppId,
                ComponentSecret = configItem.Secret,
            });
            ProcessResult(result);
        }

        public async Task AuthNotify(Auth auth, string body)
        {
            CheckHelper.IsNotNull(body, name: nameof(body));

            var client = new WechatApiClient(string.Empty, string.Empty);
            var baseEvent = client.DeserializeEventFromXml(body);
            CheckHelper.IsNotNull(baseEvent, $"报文格式不正确，内容：{body}");
            CheckHelper.IsNotNull(baseEvent.ComponentAppId, $"无法获取{nameof(baseEvent.ComponentAppId)}");

            client = await GetComponentClient(baseEvent.ComponentAppId);

            var isValid = client.VerifyEventSignatureFromXml(auth.timestamp, auth.nonce, body, auth.msg_signature);
            CheckHelper.IsTrue(isValid, "回调数据验证失败");

            baseEvent = client.DeserializeEventFromXml(body, true);
            CheckHelper.IsNotNull(baseEvent, $"解密失败，内容：{body}");
            _logger.LogDebug($"解密报文：{baseEvent.ToJsonString()}");

            if (baseEvent.InfoType == "component_verify_ticket")
            {
                var eventDto = client.DeserializeEventFromXml<ComponentVerifyTicketEvent>(body, true);
                var ticket = _objectMapper.Map<ComponentVerifyTicketEvent, ComponentVerifyTicketCacheItem>(eventDto);

                var oldTicket = await _verifyTicketCache.GetAsync(ticket.ComponentAppId);
                if (oldTicket.IsNotNull())
                {
                    ticket.LastComponentVerifyTicket = oldTicket.ComponentVerifyTicket;
                }

                await _verifyTicketCache.SetAsync(ticket.ComponentAppId, ticket, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.FromUnixTimeSeconds(ticket.CreateTimestamp).AddHours(12),
                });
                return;
            }

            if (baseEvent.InfoType == "unauthorized")
            {
                var eventDto = client.DeserializeEventFromXml<ComponentUnauthorizedEvent>(body, true);
                var appAuth = await _wechatComponentAuthRespository.GetAsync(p => p.AuthAppId == eventDto.AuthorizerAppId);
                appAuth.UnAuthorized = true;

                await _updateAccessTokenCache.SetAsync(eventDto.AuthorizerAppId, new UpdateAccessTokenArgs
                {
                    AppId = eventDto.AuthorizerAppId,
                    UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                }, options: new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });

                return;
            }

            if (baseEvent.InfoType == "updateauthorized" || baseEvent.InfoType == "authorized")
            {
                //已在AuthSuccess处理
                return;
            }

            _logger.LogCritical($"未知的授权类型：{baseEvent.InfoType}，授权内容：{body}");
        }

        public async Task<string> MessageNotify(string appId, Auth auth, string body)
        {
            CheckHelper.IsNotNull(body, name: nameof(body));

            var authInfo = await _wechatComponentAuthRespository.FindAsync(p => p.AuthAppId == appId);
            CheckHelper.IsNotNull(authInfo, name: nameof(authInfo));

            var client = await GetComponentClient(authInfo.ComponentAppId);
            
            var isValid = client.VerifyEventSignatureFromXml(auth.timestamp, auth.nonce, body, auth.msg_signature);
            CheckHelper.IsTrue(isValid, "回调数据验证失败");
            
            var baseEvent = client.DeserializeEventFromXml(body, true);
            CheckHelper.IsNotNull(baseEvent, $"解密失败，内容：{body}");
            _logger.LogDebug($"解密报文：{baseEvent.ToJsonString()}");

            if (baseEvent.MessageType == "text")
            {
                var eventDto = client.DeserializeEventFromXml<TextMessageEvent>(body, true);
                _logger.LogDebug($"{eventDto.FromUserName}发送消息，内容：{eventDto.Content}");

                if(eventDto.Content == "TESTCOMPONENT_MSG_TYPE_TEXT")
                {
                    return client.SerializeEventToXml(new TextMessageEvent
                    {
                        ToUserName = eventDto.FromUserName,
                        FromUserName = eventDto.ToUserName,
                        CreateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        MessageType = "text",
                        Content = "TESTCOMPONENT_MSG_TYPE_TEXT_callback",
                    });
                }
                if (eventDto.Content == $"QUERY_AUTH_CODE:{authInfo.AuthorizationCode}")
                {
                    await client.ExecuteCgibinMessageCustomSendAsync(new CgibinMessageCustomSendRequest
                    {
                        ToUserOpenId = eventDto.FromUserName,
                        MessageType = "text",
                        MessageContentForText = new CgibinMessageCustomSendRequest.Types.TextMessage
                        {
                            Content = $"{authInfo.AuthorizationCode}_from_api"
                        }
                    });

                    return string.Empty;
                }
            }

            if(baseEvent.MessageType == "transfer_customer_service")
            {

            }

            _logger.LogCritical($"未知的通知类型：{baseEvent.MessageType}，通知内容：{body}");
            return string.Empty;
        }

        public async Task<CgibinComponentApiQueryAuthResponse> QueryAuth(string componentAppId, string authorizationCode)
        {
            var accessTokenItem = await GetAccessToken(componentAppId);

            var client = await GetComponentClient(componentAppId);
            var result = await client.ExecuteCgibinComponentApiQueryAuthAsync(new CgibinComponentApiQueryAuthRequest
            {
                ComponentAppId = componentAppId,
                ComponentAccessToken = accessTokenItem.AccessToken,
                AuthCode = authorizationCode,
            });
            ProcessResult(result);

            await _accessTokenCache.SetAsync(result.Authorization.AuthorizerAppId, new AccessTokenCacheItem
            {
                AccessToken = result.Authorization.AuthorizerAccessToken,
                ExpiresIn = result.Authorization.ExpiresIn,
                RefreshToken = result.Authorization.AuthorizerRefreshToken,
            });

            return result;
        }

        private async Task<ComponentConfigCacheItem> GetComponentConfig(string componentAppId)
        {
            var dto = await _configCache.GetAsync(componentAppId);
            if (dto.IsNotNull()) return dto;

            var config = await _wechatComponentConfigRespository.GetAsync(p => p.ComponentAppId == componentAppId);

            dto = _objectMapper.Map<WechatComponentConfig, ComponentConfigCacheItem>(config);
            await _configCache.SetAsync(componentAppId, dto);

            return dto;
        }

        private async Task<WechatApiClient> GetComponentClient(string componentAppId)
        {
            var componentConfigDto = await GetComponentConfig(componentAppId);
            CheckHelper.IsNotNull(componentConfigDto.Secret, $"第三方平台：{componentAppId}的密钥不能为空");

            var client = new WechatApiClient(new WechatApiClientOptions
            {
                AppId = componentAppId,
                AppSecret = componentConfigDto.Secret,
                PushEncodingAESKey = componentConfigDto.EncodingAESKey,
                PushToken = componentConfigDto.Token,
            });

            return client;
        }

        private void ProcessResult(WechatApiResponse response)
        {
            CheckHelper.IsTrue(response.IsSuccessful(), $"请求发生错误，原因：[{response.ErrorCode}]{response.ErrorMessage}");
        }
    }
}
