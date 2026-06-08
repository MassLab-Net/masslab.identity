namespace MassLab.Identity.Settings;

public static class IdentitySettings
{
    private const string Prefix = "Identity";

    public const string ExternalLoginPrefix = Prefix + ".ExternalLogin";

    public static class ExternalLogin
    {
        public static class Google
        {
            public const string Enabled = ExternalLoginPrefix + ".Google.Enabled";
            public const string ClientId = ExternalLoginPrefix + ".Google.ClientId";
            public const string ClientSecret = ExternalLoginPrefix + ".Google.ClientSecret";
        }

        public static class EntraId
        {
            public const string Enabled = ExternalLoginPrefix + ".EntraId.Enabled";
            public const string TenantId = ExternalLoginPrefix + ".EntraId.TenantId";
            public const string ClientId = ExternalLoginPrefix + ".EntraId.ClientId";
            public const string ClientSecret = ExternalLoginPrefix + ".EntraId.ClientSecret";
        }
    }
}
