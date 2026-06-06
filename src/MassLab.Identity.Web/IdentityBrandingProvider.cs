using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using MassLab.Identity.Localization;

namespace MassLab.Identity.Web;

[Dependency(ReplaceServices = true)]
public class IdentityBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<IdentityResource> _localizer;

    public IdentityBrandingProvider(IStringLocalizer<IdentityResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
