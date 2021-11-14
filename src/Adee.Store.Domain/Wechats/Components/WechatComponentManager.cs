using Adee.Store.Wechats.Components.Jobs.UpdateAccessToken;
using Adee.Store.Wechats.Components.Models;
using Adee.Store.Wechats.Components.Repositorys;
using Adee.Store.Wechats.OffiAccount.Messages.Repositorys;
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
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Adee.Store.Wechats.Components
{
    public class WechatComponentManager : DomainService, IWechatComponentManager, ITransientDependency
    {
        private readonly IDistributedCache<ComponentConfigCacheItem> _configCache;
        private readonly IDistributedCache<AccessTokenCacheItem> _accessTokenCache;
        private readonly IDistributedCache<ComponentVerifyTicketCacheItem> _verifyTicketCache;
        private readonly IDistributedCache<UpdateAccessTokenArgs> _updateAccessTokenCache;
        private readonly IDistributedCache<ReplyMessageCacheItem> _replyMessageCache;
        private readonly IRepository<WechatComponentConfig> _wechatComponentConfigRespository;
        private readonly IRepository<WechatComponentAuth> _wechatComponentAuthRespository;
        private readonly IRepository<WechatOffiAccoutReplyMessage> _wechatOffiAccountReplyMessageRespository;
        private readonly IDataFilter _dataFilter;
        private readonly IObjectMapper _objectMapper;

        public WechatComponentManager(
            IDistributedCache<ComponentConfigCacheItem> configCache,
            IDistributedCache<AccessTokenCacheItem> accessTokenCache,
            IDistributedCache<ComponentVerifyTicketCacheItem> verifyTicketCache,
            IDistributedCache<UpdateAccessTokenArgs> updateAccessTokenCache,
            IDistributedCache<ReplyMessageCacheItem> replyMessageCache,
            IRepository<WechatComponentConfig> wechatComponentConfigRespository,
            IRepository<WechatComponentAuth> wechatComponentAuthRespository,
            IRepository<WechatOffiAccoutReplyMessage> wechatOffiAccountReplyMessageRespository,
            IDataFilter dataFilter,
            IObjectMapper objectMapper)
        {
            _configCache = configCache;
            _accessTokenCache = accessTokenCache;
            _verifyTicketCache = verifyTicketCache;
            _updateAccessTokenCache = updateAccessTokenCache;
            _replyMessageCache = replyMessageCache;
            _wechatComponentConfigRespository = wechatComponentConfigRespository;
            _wechatComponentAuthRespository = wechatComponentAuthRespository;
            _wechatOffiAccountReplyMessageRespository = wechatOffiAccountReplyMessageRespository;
            _dataFilter = dataFilter;
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
            if (accessTokenItem.IsNotNull() && accessTokenItem.AccessToken.IsNotNull())
            {
                return accessTokenItem;
            }

            WechatComponentAuth auth = null;
            using (_dataFilter.Disable<IMultiTenant>())
            {
                auth = await _wechatComponentAuthRespository.FindAsync(p => p.AuthAppId == appId);
            }
            CheckHelper.IsNotNull(auth, name: nameof(auth));

            var response = await QueryAuth(auth.ComponentAppId, auth.AuthorizationCode);
            CheckHelper.IsNotNull(response, name: nameof(response));

            auth.AuthorizerRefreshToken = response.Authorization.AuthorizerRefreshToken;
            await _wechatComponentAuthRespository.UpdateAsync(auth, autoSave: true);

            return new AccessTokenCacheItem
            {
                AccessToken = response.Authorization.AuthorizerAccessToken,
                ExpiresIn = response.Authorization.ExpiresIn,
                RefreshToken = response.Authorization.AuthorizerRefreshToken,
                AuthorizationCode = auth.AuthorizationCode,
            };
        }

        public async Task<AccessTokenCacheItem> UpdateAccessToken(string appId)
        {
            WechatComponentAuth auth = null;
            using (_dataFilter.Disable<IMultiTenant>())
            {
                auth = await _wechatComponentAuthRespository.FindAsync(p => p.AuthAppId == appId);
            }
            CheckHelper.IsNotNull(auth, name: nameof(auth));

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
                AuthorizationCode = auth.AuthorizationCode,
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
                TenantId = CurrentTenant.Id
            }.ToJsonString();
            var url = $"{domain}/api/store/wechat-component/auth-success?Data={HttpUtility.UrlEncode(data)}";
            url = HttpUtility.UrlEncode(url);

            var authType = $"auth_type={(int)dto.AuthType}";
            var auth = await _wechatComponentAuthRespository.FindAsync(p => p.TenantId == CurrentTenant.Id);
            if (auth.IsNotNull())
            {
                authType = $"biz_appid={auth.AuthAppId}";
            }

            if (dto.IsMobile)
            {
                return $"https://mp.weixin.qq.com/safe/bindcomponent?action=bindcomponent&no_scan=1&component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.PreAuthCode}&redirect_uri={url}&{authType}#wechat_redirect";
            }
            else
            {
                return $"https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid={dto.ComponentAppId}&pre_auth_code={preAuthCode.PreAuthCode}&redirect_uri={url}&{authType}";
            }
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

        public async Task AuthNotify(EncryptNotify notify, string body)
        {
            CheckHelper.IsNotNull(body, name: nameof(body));

            var client = new WechatApiClient(string.Empty, string.Empty);
            var baseEvent = client.DeserializeEventFromXml(body);
            CheckHelper.IsNotNull(baseEvent, $"报文格式不正确，内容：{body}");
            CheckHelper.IsNotNull(baseEvent.ComponentAppId, $"无法获取{nameof(baseEvent.ComponentAppId)}");

            client = await GetComponentClient(baseEvent.ComponentAppId);

            var isValid = client.VerifyEventSignatureFromXml(notify.timestamp, notify.nonce, body, notify.msg_signature);
            CheckHelper.IsTrue(isValid, "回调数据验证失败");

            baseEvent = client.DeserializeEventFromXml(body, true);
            CheckHelper.IsNotNull(baseEvent, $"解密失败，内容：{body}");
            Logger.LogDebug($"解密报文：{baseEvent.ToJsonString()}");

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

                using (_dataFilter.Disable<IMultiTenant>())
                {
                    var appAuth = await _wechatComponentAuthRespository.FindAsync(p => p.AuthAppId == eventDto.AuthorizerAppId);
                    if (appAuth.IsNotNull())
                    {
                        appAuth.UnAuthorized = true;
                    }
                }

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

            Logger.LogWarning($"未知的授权类型：{baseEvent.InfoType}，授权内容：{body}");
        }

        public async Task<string> MessageNotify(string appId, EncryptNotify notify, string body)
        {
            CheckHelper.IsNotNull(body, name: nameof(body));

            WechatComponentAuth auth = null;
            using (_dataFilter.Disable<IMultiTenant>())
            {
                auth = await _wechatComponentAuthRespository.FindAsync(p => p.AuthAppId == appId);
            }
            CheckHelper.IsNotNull(auth, name: nameof(auth));

            var client = await GetComponentClient(auth.ComponentAppId);

            var isValid = client.VerifyEventSignatureFromXml(notify.timestamp, notify.nonce, body, notify.msg_signature);
            CheckHelper.IsTrue(isValid, "回调数据验证失败");

            var baseEvent = client.DeserializeEventFromXml(body, true);
            CheckHelper.IsNotNull(baseEvent, $"解密失败，内容：{body}");
            Logger.LogDebug($"解密报文：{baseEvent.ToJsonString()}");

            if (baseEvent.MessageType == NotifyMessageType.Text)
            {
                return await ReplyMessage4Text(appId, auth.TenantId, body, client);
            }

            if (baseEvent.MessageType == NotifyMessageType.TransferCustomerService)
            {

            }

            Logger.LogCritical($"未知的通知类型：{baseEvent.MessageType}，通知内容：{body}");
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
                AuthorizationCode = authorizationCode,
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

        private async Task<string> ReplyMessage4Text(string appId, Guid? tenantId, string body, WechatApiClient client)
        {
            WechatOffiAccoutReplyMessage replyMessage = null;

            var eventDto = client.DeserializeEventFromXml<TextMessageEvent>(body, true);
            Logger.LogInformation($"{eventDto.FromUserName}发送消息，内容：{eventDto.Content}");

            var replyMessageKey = $"{appId}-{eventDto.Content.ToMd5()}";

            var replyMessageCache = await _replyMessageCache.GetAsync(replyMessageKey);
            if (replyMessageCache.IsNotNull()) return replyMessageCache.Content;

            using (CurrentTenant.Change(tenantId))
            {
                var replyMessages = await _wechatOffiAccountReplyMessageRespository.GetListAsync(p => p.Keyword.Contains(eventDto.Content));

                replyMessage = replyMessages.FirstOrDefault(p => p.MatchType == OffiAccount.MatchType.Full);
                if (replyMessage.IsNull()) replyMessage = replyMessages.FirstOrDefault(p => p.MatchType == OffiAccount.MatchType.StartLike);
                if (replyMessage.IsNull()) replyMessage = replyMessages.FirstOrDefault(p => p.MatchType == OffiAccount.MatchType.EndLike);
                if (replyMessage.IsNull()) replyMessage = replyMessages.FirstOrDefault(p => p.MatchType == OffiAccount.MatchType.Like);

                if (replyMessage.IsNull())
                {
                    replyMessage = await _wechatOffiAccountReplyMessageRespository.FindAsync(p => p.MatchType == OffiAccount.MatchType.Default);
                }
            }

            if (replyMessage.IsNull()) return "success";

            replyMessageCache = new ReplyMessageCacheItem
            {
                Keyword = eventDto.Content,
            };
            var isReply = false;

            if (replyMessage.MessageType == ReplyMessageType.Text)
            {
                replyMessageCache.Content = client.SerializeEventToXml(new TextMessageReply
                {
                    FromUserName = eventDto.ToUserName,
                    ToUserName = eventDto.FromUserName,
                    CreateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    MessageType = ReplyMessageType.Text,
                    Content = replyMessage.MessageContent,
                });

                isReply = true;
            }

            if (replyMessage.MessageType == ReplyMessageType.Image)
            {
                replyMessageCache.Content = client.SerializeEventToXml(new ImageMessageReply
                {
                    FromUserName = eventDto.ToUserName,
                    ToUserName = eventDto.FromUserName,
                    CreateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    MessageType = ReplyMessageType.Image,
                    Image = new ImageMessageReply.Types.Image
                    {
                        MediaId = replyMessage.MessageContent,
                    }
                });

                isReply = true;
            }

            if (replyMessage.MessageType == ReplyMessageType.Voice)
            {
                replyMessageCache.Content = client.SerializeEventToXml(new VoiceMessageReply
                {
                    FromUserName = eventDto.ToUserName,
                    ToUserName = eventDto.FromUserName,
                    CreateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    MessageType = ReplyMessageType.Voice,
                    Voice = new VoiceMessageReply.Types.Voice
                    {
                        MediaId = replyMessage.MessageContent,
                    },
                });

                isReply = true;
            }

            if (replyMessage.MessageType == ReplyMessageType.Video)
            {
                replyMessageCache.Content = client.SerializeEventToXml(new VideoMessageReply
                {
                    FromUserName = eventDto.ToUserName,
                    ToUserName = eventDto.FromUserName,
                    CreateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    MessageType = ReplyMessageType.Video,
                    Video = replyMessage.MessageContent.AsObject<VideoMessageReply.Types.Video>()
                });

                isReply = true;
            }

            if (replyMessage.MessageType == ReplyMessageType.Music)
            {
                replyMessageCache.Content = client.SerializeEventToXml(new MusicMessageReply
                {
                    FromUserName = eventDto.ToUserName,
                    ToUserName = eventDto.FromUserName,
                    CreateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    MessageType = ReplyMessageType.Video,
                    Music = replyMessage.MessageContent.AsObject<MusicMessageReply.Types.Music>()
                });

                isReply = true;
            }

            if (replyMessage.MessageType == ReplyMessageType.CustomerService)
            {
                var replyContent = "未知的消息内容";

                if (eventDto.Content.StartsWith($"QUERY_AUTH_CODE:"))
                {
                    var accessToken = await _accessTokenCache.GetAsync(appId);

                    var isValid = eventDto.Content.StartsWith($"QUERY_AUTH_CODE:{accessToken.AuthorizationCode}");
                    replyContent = $"无效的AuthorizationCode，内容：{eventDto.Content}";

                    await client.ExecuteCgibinMessageCustomSendAsync(new CgibinMessageCustomSendRequest
                    {
                        ToUserOpenId = eventDto.FromUserName,
                        MessageType = NotifyMessageType.Text,
                        MessageContentForText = new CgibinMessageCustomSendRequest.Types.TextMessage
                        {
                            Content = isValid ? $"{accessToken.AuthorizationCode}_from_api" : replyContent,
                        }
                    });

                    if (isValid)
                    {
                        replyContent = string.Empty;
                    }
                }

                replyMessageCache.Content = client.SerializeEventToXml(new TextMessageReply
                {
                    FromUserName = eventDto.ToUserName,
                    ToUserName = eventDto.FromUserName,
                    CreateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    MessageType = ReplyMessageType.Text,
                    Content = replyContent,
                });

                isReply = true;
            }

            if (isReply == true)
            {
                await _replyMessageCache.SetAsync(replyMessageKey, replyMessageCache, options: new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                });

                return replyMessageCache.Content;
            }

            Logger.LogWarning($"未处理的消息类型：{replyMessage.MessageType}，id：{replyMessage.Id}");
            return "success";
        }
    }
}
