using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Adee.Store
{
    [Dependency(ReplaceServices = true)]
    public class StoreBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Store";
    }
}
