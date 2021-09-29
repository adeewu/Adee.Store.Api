using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adee.Store.Domain.Tenants;
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
        private readonly ICurrentTenantExt _currentTenantExt;

        public PayParameterAppService(
            IRepository<PayParameter, Guid> payParameterRepository,
            ICurrentTenantExt currentTenantExt
            ) : base(payParameterRepository)
        {
            _currentTenantExt = currentTenantExt;
        }

        /// <summary>
        /// 设置版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task SetPayParameterVersion(long version)
        {
            await _currentTenantExt.SetPaypameterVersion(version);
        }

        protected override PayParameter MapToEntity(CreateUpdatePayParameterDto createInput)
        {
            var model = base.MapToEntity(createInput);
            model.Version = DateTimeOffset.Now.ToUnixTimeSeconds();

            return model;
        }
    }
}
