using Volo.Abp.Settings;

namespace Adee.Store.Settings
{
    public class StoreSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition(StoreSettings.PayParameterVersionKey));
        }
    }
}
