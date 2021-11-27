using Adee.Store.Attributes;
using Adee.Store.IdentityServer;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.Clients;

namespace Adee.Store.IdentityServers.Clients
{
    /// <summary>
    /// 标识客户端
    /// </summary>
    [ApiGroup(ApiGroupType.IdentityServer)]
    public class IdentityServerClientAppService : StoreAppService, IIdentityServerClientAppService
    {
        private readonly IdenityServerClientManager _idenityServerClientManager;

        public IdentityServerClientAppService(IdenityServerClientManager idenityServerClientManager)
        {
            _idenityServerClientManager = idenityServerClientManager;

        }

        /// <summary>
        /// 获取客户端集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ClientListOutput>> GetListAsync(ClientListInput input)
        {
            var list = await _idenityServerClientManager.GetListAsync(
                input.SkipCount,
                input.PageSize,
                input.Filter,
                true);
            var totalCount = await _idenityServerClientManager.GetCountAsync(input.Filter);
            return new PagedResultDto<ClientListOutput>(totalCount,
                ObjectMapper.Map<List<Client>, List<ClientListOutput>>(list));
        }

        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task CreateAsync(CreateClientInput input)
        {
            return _idenityServerClientManager.CreateAsync(input.ClientId, input.ClientName, input.Description, input.AllowedGrantTypes);
        }

        /// <summary>
        /// 删除客户端
        /// </summary>
        /// <returns></returns>
        public Task DeleteAsync(IdDto input)
        {
            return _idenityServerClientManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 更新客户端
        /// </summary>
        /// <returns></returns>
        public Task UpdateBasicDataAsync(UpdataBasicDataInput input)
        {
            return _idenityServerClientManager.UpdateBasicDataAsync(
                input.ClientId,
                input.ClientName,
                input.Description,
                input.ClientUri,
                input.LogoUri,
                input.Enabled,
                input.ProtocolType,
                input.RequireClientSecret,
                input.RequireConsent,
                input.AllowRememberConsent,
                input.AlwaysIncludeUserClaimsInIdToken,
                input.RequirePkce,
                input.AllowPlainTextPkce,
                input.RequireRequestObject,
                input.AllowAccessTokensViaBrowser,
                input.FrontChannelLogoutUri,
                input.FrontChannelLogoutSessionRequired,
                input.BackChannelLogoutUri,
                input.BackChannelLogoutSessionRequired,
                input.AllowOfflineAccess,
                input.IdentityTokenLifetime,
                input.AllowedIdentityTokenSigningAlgorithms,
                input.AccessTokenLifetime,
                input.AuthorizationCodeLifetime,
                input.ConsentLifetime,
                input.AbsoluteRefreshTokenLifetime,
                input.RefreshTokenUsage,
                input.UpdateAccessTokenClaimsOnRefresh,
                input.RefreshTokenExpiration,
                input.AccessTokenType,
                input.EnableLocalLogin,
                input.IncludeJwtId,
                input.AlwaysSendClientClaims,
                input.ClientClaimsPrefix,
                input.PairWiseSubjectSalt,
                input.UserSsoLifetime,
                input.UserCodeType,
                input.DeviceCodeLifetime,
                input.SlidingRefreshTokenLifetime,
                input.Secret,
                input.SecretType,
                input.AllowedGrantTypes
            );
        }

        /// <summary>
        /// 更新客户端作用域
        /// </summary>
        /// <returns></returns>
        public Task UpdateScopesAsync(UpdateScopeInput input)
        {
            return _idenityServerClientManager.UpdateScopesAsync(input.ClientId, input.Scopes);
        }

        /// <summary>
        /// 新增客户端回调地址
        /// </summary>
        public Task AddRedirectUriAsync(AddRedirectUriInput input)
        {
            return _idenityServerClientManager.AddRedirectUriAsync(input.ClientId, input.Uri);
        }

        /// <summary>
        /// 删除客户端回调地址
        /// </summary>
        public Task RemoveRedirectUriAsync(RemoveRedirectUriInput input)
        {
            return _idenityServerClientManager.RemoveRedirectUriAsync(input.ClientId, input.Uri);
        }

        /// <summary>
        /// 新增客户端注销回调地址
        /// </summary>
        public Task AddLogoutRedirectUriAsync(AddRedirectUriInput input)
        {
            return _idenityServerClientManager.AddLogoutRedirectUriAsync(input.ClientId, input.Uri);
        }

        /// <summary>
        /// 删除客户端注销回调地址
        /// </summary>
        public Task RemoveLogoutRedirectUriAsync(RemoveRedirectUriInput input)
        {
            return _idenityServerClientManager.RemoveLogoutRedirectUriAsync(input.ClientId, input.Uri);
        }

        /// <summary>
        /// 添加客户端跨域
        /// </summary>
        public Task AddCorsAsync(AddCorsInput input)
        {
            return _idenityServerClientManager.AddCorsAsync(input.ClientId, input.Origin);
        }

        /// <summary>
        /// 删除客户端跨域
        /// </summary>
        public Task RemoveCorsAsync(RemoveCorsInput input)
        {
            return _idenityServerClientManager.RemoveCorsAsync(input.ClientId, input.Origin);
        }

        /// <summary>
        /// 禁用客户端
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task EnabledAsync(EnabledInput input)
        {
            return _idenityServerClientManager.EnabledAsync(input.ClientId, input.Enabled);
        }


    }
}