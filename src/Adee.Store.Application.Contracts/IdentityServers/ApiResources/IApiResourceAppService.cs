using Adee.Store.IdentityServers.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Adee.Store.IdentityServers
{
    public interface IApiResourceAppService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApiResourceOutput>> GetListAsync(ApiRseourceListInput input);

        /// <summary>
        /// 获取所有api resource
        /// </summary>
        /// <returns></returns>
        Task<List<ApiResourceOutput>> GetApiResources();

        /// <summary>
        /// 新增 ApiResource
        /// </summary>
        /// <returns></returns>
        Task CreateAsync(CreateApiResourceInput input);

        /// <summary>
        /// 删除 ApiResource
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(IdDto input);

        /// <summary>
        /// 更新 ApiResource
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(UpdateApiResourceInput input);
    }
}