using Adee.Store.IdentityServers.ApiScopes.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Adee.Store.IdentityServers.ApiScopes
{
    public interface IApiScopeAppService : IApplicationService
    {
        Task<PagedResultDto<ApiScopeListOutput>> GetListAsync(ApiScopeListInput input);

        Task CreateAsync(CreateApiScopeInput input);

        Task UpdateAsync(UpdateCreateApiScopeInput input);

        Task DeleteAsync(IdDto input);

        Task<List<KeyValuePair<string, string>>> GetFindAllAsync();
    }
}