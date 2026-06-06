using MassLab.Identity.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MassLab.Identity.Web.Pages;

public abstract class IdentityPageModel : AbpPageModel
{
    protected IdentityPageModel()
    {
        LocalizationResourceType = typeof(IdentityResource);
    }
}
