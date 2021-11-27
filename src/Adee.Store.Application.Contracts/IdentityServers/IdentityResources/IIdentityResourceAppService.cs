using Adee.Store.IdentityServers.IdentityResources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Adee.Store.IdentityServers.IdentityResources
{
    public interface IIdentityResourceAppService : IApplicationService
    {
        /// <summary>
        /// 分页获取IdentityResource
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityResourceListOutput>> GetListAsync(IdentityResourceListInput input);

        /// <summary>
        /// 获取所有资源
        /// </summary>
        /// <returns></returns>
        Task<List<IdentityResourceListOutput>> GetFindAllAsync();

        /// <summary>
        /// 创建IdentityResource
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAsync(CreateIdentityResourceInput input);

        /// <summary>
        /// 更新IdentityResource
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(UpdateIdentityResourceInput input);

        /// <summary>
        /// 删除IdentityResource
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(IdDto input);
    }
}