namespace MassLab.Identity.Permissions;

public static class IdentityPermissions
{
    public const string GroupName = "MassLab";

    public const string OrdersRead = MassLabServicePermissions.OrdersRead;
    public const string OrdersWrite = MassLabServicePermissions.OrdersWrite;
    public const string ProductsRead = MassLabServicePermissions.ProductsRead;
    public const string ProductsWrite = MassLabServicePermissions.ProductsWrite;
    public const string ProductsInventory = MassLabServicePermissions.ProductsInventory;
    public const string ProductsSettings = MassLabServicePermissions.ProductsSettings;
    public const string ExternalLoginProvidersManage = MassLabServicePermissions.ExternalLoginProvidersManage;

    public static string[] All => MassLabServicePermissions.All;
}
