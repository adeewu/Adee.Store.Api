using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.SettingManagement;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付参数服务
    /// </summary>    
    public class PayParameterAppService : CrudAppService<PayParameter, PayParameterDto, Guid, PayParameterListDto, CreateUpdatePayParameterDto>
    {
        public PayParameterAppService(
            IRepository<PayParameter, Guid> payParameterRepository
            ) : base(payParameterRepository)
        {
            
        }

        protected override PayParameter MapToEntity(CreateUpdatePayParameterDto createInput)
        {
            var model = base.MapToEntity(createInput);
            model.Version = DateTimeOffset.Now.ToUnixTimeSeconds();

            return model;
        }
    }
}
