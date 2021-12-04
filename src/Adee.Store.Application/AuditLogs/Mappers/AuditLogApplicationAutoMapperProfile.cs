using AutoMapper;
using Volo.Abp.AuditLogging;

namespace Adee.Store.AuditLogs.Mappers
{
    public class AuditLogApplicationAutoMapperProfile : Profile
    {
        public AuditLogApplicationAutoMapperProfile()
        {
            CreateMap<AuditLog, AuditLogListOutput>();
        }
    }
}