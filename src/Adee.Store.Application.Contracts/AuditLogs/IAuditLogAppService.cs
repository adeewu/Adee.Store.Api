using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Adee.Store.AuditLogs
{
    public interface IAuditLogAppService : IApplicationService
    {
        /// <summary>
        /// 分页查询审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AuditLogListOutput>> GetListAsync(AuditLogListInput input);
    }
}