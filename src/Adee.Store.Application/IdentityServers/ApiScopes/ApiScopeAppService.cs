using Adee.Store.Attributes;
using Adee.Store.IdentityServer;
using Adee.Store.IdentityServers.ApiScopes.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.ApiScopes;

namespace Adee.Store.IdentityServers.ApiScopes
{
    /// <summary>
    /// api作用域
    /// </summary>
    [ApiGroup(ApiGroupType.IdentityServer)]
    public class ApiScopeAppService : StoreAppService, IApiScopeAppService
    {
        private readonly IdenityServerApiScopeManager _idenityServerApiScopeManager;
        private readonly IdentityResourceManager _identityResourceManager;

        public ApiScopeAppService(IdenityServerApiScopeManager idenityServerApiScopeManager, IdentityResourceManager identityResourceManager)
        {
            _idenityServerApiScopeManager = idenityServerApiScopeManager;
            _identityResourceManager = identityResourceManager;
        }

        /// <summary>
        /// 获取api作用域集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ApiScopeListOutput>> GetListAsync(ApiScopeListInput input)
        {
            var list = await _idenityServerApiScopeManager.GetListAsync(
                input.GetSkipCount(),
                input.PageSize,
                input.Filter,
                false
                );

            var totalCount = await _idenityServerApiScopeManager.GetCountAsync(input.Filter);

            return new PagedResultDto<ApiScopeListOutput>(totalCount, ObjectMapper.Map<List<ApiScope>, List<ApiScopeListOutput>>(list));
        }

        /// <summary>
        /// 新增api作用域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task CreateAsync(CreateApiScopeInput input)
        {
            return _idenityServerApiScopeManager.CreateAsync(
                input.Name,
                input.DisplayName,
                input.Description,
                input.Enabled,
                input.Required,
                input.Emphasize,
                input.ShowInDiscoveryDocument
                );
        }

        /// <summary>
        /// 更新api作用域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdateCreateApiScopeInput input)
        {
            return _idenityServerApiScopeManager.UpdateAsync(
                input.Name,
                input.DisplayName,
                input.Description,
                input.Enabled,
                input.Required,
                input.Emphasize,
                input.ShowInDiscoveryDocument
                );
        }

        /// <summary>
        /// 删除api作用域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdDto input)
        {
            return _idenityServerApiScopeManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 获取所有api作用域
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetFindAllAsync()
        {
            var apiScopes = await _idenityServerApiScopeManager.FindAllAsync();

            var identityResoure = await _identityResourceManager.GetAllAsync();

            var result = new List<KeyValuePair<string, string>>();
            result.AddRange(apiScopes.Select(e => new KeyValuePair<string, string>(e.Name, e.DisplayName)).ToList());
            result.AddRange(identityResoure.Select(e => new KeyValuePair<string, string>(e.Name, e.DisplayName)).ToList());
            return result;
        }
    }
}