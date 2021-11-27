using Adee.Store.Attributes;
using EasyAbp.Abp.SettingUi;
using EasyAbp.Abp.SettingUi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adee.Store.Settings
{
    /// <summary>
    /// ϵͳ����
    /// </summary>
    [ApiGroup(ApiGroupType.SystemBase)]
    public class SettingAppService : StoreAppService, ISettingAppService
    {
        private readonly ISettingUiAppService _settingUiAppService;

        public SettingAppService(ISettingUiAppService settingUiAppService)
        {
            _settingUiAppService = settingUiAppService;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SettingGroup>> GetAsync()
        {
            return await _settingUiAppService.GroupSettingDefinitions();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateSettingInput input)
        {
            await _settingUiAppService.SetSettingValues(input.Values);
        }
    }
}