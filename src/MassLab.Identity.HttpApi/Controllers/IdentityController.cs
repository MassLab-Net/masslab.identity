using MassLab.Identity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MassLab.Identity.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class IdentityController : AbpControllerBase
{
    protected IdentityController()
    {
        LocalizationResource = typeof(IdentityResource);
    }
}
