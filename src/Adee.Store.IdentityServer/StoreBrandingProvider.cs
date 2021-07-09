using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Adee.Store
{
    [Dependency(ReplaceServices = true)]
    public class StoreBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Store";
    }
}
