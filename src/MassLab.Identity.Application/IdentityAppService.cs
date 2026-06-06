using MassLab.Identity.Localization;
using Volo.Abp.Application.Services;

namespace MassLab.Identity;

/* Inherit your application services from this class.
 */
public abstract class IdentityAppService : ApplicationService
{
    protected IdentityAppService()
    {
        LocalizationResource = typeof(IdentityResource);
    }
}
