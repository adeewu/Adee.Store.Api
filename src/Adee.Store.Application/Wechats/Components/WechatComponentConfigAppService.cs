using Adee.Store.Attributes;
using Adee.Store.Wechats.Components.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Wechats.Components
{
    /// <summary>
    /// 第三方平台配置
    /// </summary>
    [ApiGroup(ApiGroupType.WechatComponent)]
    public class WechatComponentConfigAppService : CrudAppService<WechatComponentConfig, WechatComponentConfigDto, Guid, WechatComponentConfigListDto, CreateUpdateWechatComponentConfigDto>
    {
        public WechatComponentConfigAppService(
            IRepository<WechatComponentConfig, Guid> wechatComponentConfigRepository
            )
            : base(wechatComponentConfigRepository)
        {

        }

        protected override async Task<IQueryable<WechatComponentConfig>> CreateFilteredQueryAsync(WechatComponentConfigListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            return query
                .WhereIf(input.ComponentAppId.IsNullOrWhiteSpace() == false, p => p.ComponentAppId == input.ComponentAppId)
                .WhereIf(input.IsDisabled.HasValue, p => p.IsDisabled == input.IsDisabled);
        }
    }
}
