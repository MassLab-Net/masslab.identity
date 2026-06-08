using MassLab.Identity.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MassLab.Identity.Permissions;

public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(IdentityPermissions.GroupName);

        myGroup.AddPermission(IdentityPermissions.OrdersRead, L("Permission:OrdersRead"));
        myGroup.AddPermission(IdentityPermissions.OrdersWrite, L("Permission:OrdersWrite"));
        myGroup.AddPermission(IdentityPermissions.ProductsRead, L("Permission:ProductsRead"));
        myGroup.AddPermission(IdentityPermissions.ProductsWrite, L("Permission:ProductsWrite"));
        myGroup.AddPermission(IdentityPermissions.ProductsInventory, L("Permission:ProductsInventory"));
        myGroup.AddPermission(IdentityPermissions.ProductsSettings, L("Permission:ProductsSettings"));
        myGroup.AddPermission(IdentityPermissions.ExternalLoginProvidersManage, L("Permission:ExternalLoginProvidersManage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
