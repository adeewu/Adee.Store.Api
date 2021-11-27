using Adee.Store.Attributes;
using Adee.Store.IdentityServer;
using Adee.Store.IdentityServers.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.ApiResources;

namespace Adee.Store.IdentityServers.ApiResources
{
    /// <summary>
    /// api资源
    /// </summary>
    [ApiGroup(ApiGroupType.IdentityServer)]
    public class ApiResourceAppService : StoreAppService, IApiResourceAppService
    {
        private readonly IdenityServerApiResourceManager _idenityServerApiResourceManager;

        public ApiResourceAppService(IdenityServerApiResourceManager idenityServerApiResourceManager)
        {
            _idenityServerApiResourceManager = idenityServerApiResourceManager;
        }

        /// <summary>
        /// 分页获取api资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ApiResourceOutput>> GetListAsync(ApiRseourceListInput input)
        {
            var list = await _idenityServerApiResourceManager.GetListAsync(
                input.GetSkipCount(),
                input.PageSize,
                input.Filter,
                true);

            var totalCount = await _idenityServerApiResourceManager.GetCountAsync(input.Filter);
            return new PagedResultDto<ApiResourceOutput>(totalCount, ObjectMapper.Map<List<ApiResource>, List<ApiResourceOutput>>(list));
        }

        /// <summary>
        /// 获取所有api资源
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApiResourceOutput>> GetApiResources()
        {
            var list = await _idenityServerApiResourceManager.GetResources(false);

            return ObjectMapper.Map<List<ApiResource>, List<ApiResourceOutput>>(list);
        }

        /// <summary>
        /// 新增api资源
        /// </summary>
        /// <returns></returns>
        public Task CreateAsync(CreateApiResourceInput input)
        {
            return _idenityServerApiResourceManager.CreateAsync(
                GuidGenerator.Create(),
                input.Name,
                input.DisplayName,
                input.Description,
                input.Enabled,
                input.AllowedAccessTokenSigningAlgorithms,
                input.ShowInDiscoveryDocument,
                input.Secret
            );
        }

        /// <summary>
        /// 删除api资源
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync(IdDto input)
        {
            await _idenityServerApiResourceManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 更新api资源
        /// </summary>
        /// <returns></returns>
        public Task UpdateAsync(UpdateApiResourceInput input)
        {
            return _idenityServerApiResourceManager.UpdateAsync(
                input.Name,
                input.DisplayName,
                input.Description,
                input.Enabled,
                input.AllowedAccessTokenSigningAlgorithms,
                input.ShowInDiscoveryDocument,
                input.Secret,
                input.ApiScopes
            );
        }
    }
}