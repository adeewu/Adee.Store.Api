using Adee.Store.Attributes;
using Adee.Store.IdentityServer;
using Adee.Store.IdentityServers.IdentityResources;
using Adee.Store.IdentityServers.IdentityResources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Adee.Store.IdentityServers.Mappers.IdentityResources
{
    /// <summary>
    /// 标识资源
    /// </summary>
    [ApiGroup(ApiGroupType.IdentityServer)]
    public class IdentityResourceAppService : StoreAppService, IIdentityResourceAppService
    {
        private readonly IdentityResourceManager _identityResourceManager;

        public IdentityResourceAppService(IdentityResourceManager identityResourceManager)
        {
            _identityResourceManager = identityResourceManager;
        }

        /// <summary>
        /// 分页获取标识资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<IdentityResourceListOutput>> GetListAsync(IdentityResourceListInput input)
        {
            var list = await _identityResourceManager.GetListAsync(
                input.SkipCount,
                input.PageSize,
                input.Filter,
                true
                );

            var totalCount = await _identityResourceManager.GetCountAsync(input.Filter);

            return new PagedResultDto<IdentityResourceListOutput>(totalCount, ObjectMapper.Map<List<IdentityResource>, List<IdentityResourceListOutput>>(list));
        }

        /// <summary>
        /// 获取所有标识资源
        /// </summary>
        /// <returns></returns>
        public async Task<List<IdentityResourceListOutput>> GetFindAllAsync()
        {
            var list = await _identityResourceManager.GetAllAsync();
            return ObjectMapper.Map<List<IdentityResource>, List<IdentityResourceListOutput>>(list);
        }

        /// <summary>
        /// 创建标识资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task CreateAsync(CreateIdentityResourceInput input)
        {
            return _identityResourceManager.CreateAsync(input.Name, input.DisplayName, input.Description,
                input.Enabled, input.Required, input.Emphasize, input.ShowInDiscoveryDocument);
        }

        /// <summary>
        /// 更新标识资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdateIdentityResourceInput input)
        {
            return _identityResourceManager.UpdateAsync(input.Name, input.DisplayName, input.Description,
                input.Enabled, input.Required, input.Emphasize, input.ShowInDiscoveryDocument);
        }

        /// <summary>
        /// 删除标识资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdDto input)
        {
            return _identityResourceManager.DeleteAsync(input.Id);
        }
    }
}