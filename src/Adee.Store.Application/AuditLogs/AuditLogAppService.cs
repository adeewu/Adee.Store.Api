using Adee.Store.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AuditLogging;

namespace Adee.Store.AuditLogs
{
    /// <summary>
    /// 审计日志
    /// </summary>
    [ApiGroup(ApiGroupType.SystemBase)]
    public class AuditLogAppService : StoreAppService, IAuditLogAppService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogAppService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        /// <summary>
        /// 分页查询审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AuditLogListOutput>> GetListAsync(AuditLogListInput input)
        {
            var list = await _auditLogRepository.GetListAsync(
                input.Sorting,
                input.PageSize,
                input.GetSkipCount(),
                input.StartTime?.Date,
                input.EndTime?.Date,
                input.HttpMethod,
                input.Url,
                null,
                input.UserName,
                input.ApplicationName,
                input.CorrelationId,
                input.MaxExecutionDuration,
                input.MinExecutionDuration,
                input.HasException,
                input.HttpStatusCode
                );

            var totalCount = await _auditLogRepository.GetCountAsync(
                input.StartTime?.Date,
                input.EndTime?.Date,
                input.HttpMethod,
                input.Url,
                null,
                input.UserName,
                input.ApplicationName,
                input.CorrelationId,
                input.MaxExecutionDuration,
                input.MinExecutionDuration,
                input.HasException,
                input.HttpStatusCode
                );

            return new PagedResultDto<AuditLogListOutput>(totalCount, ObjectMapper.Map<List<AuditLog>, List<AuditLogListOutput>>(list));
        }
    }
}