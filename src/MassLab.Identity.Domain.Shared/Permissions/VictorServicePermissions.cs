namespace MassLab.Identity.Permissions;

public static class MassLabServicePermissions
{
    public const string OrdersRead = "orders.read";
    public const string OrdersWrite = "orders.write";
    public const string ProductsRead = "products.read";
    public const string ProductsWrite = "products.write";
    public const string ProductsInventory = "products.inventory";
    public const string ProductsSettings = "products.settings";

    public static string[] All { get; } =
    {
        OrdersRead,
        OrdersWrite,
        ProductsRead,
        ProductsWrite,
        ProductsInventory,
        ProductsSettings
    };
}
