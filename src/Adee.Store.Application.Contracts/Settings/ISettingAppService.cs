using EasyAbp.Abp.SettingUi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Adee.Store.Settings
{
    public interface ISettingAppService : IApplicationService
    {
        /// <summary>
        /// 获取setting信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SettingGroup>> GetAsync();

        /// <summary>
        /// 更新setting
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(UpdateSettingInput input);
    }
}