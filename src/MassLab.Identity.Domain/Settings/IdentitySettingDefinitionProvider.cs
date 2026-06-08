using Volo.Abp.Settings;

namespace MassLab.Identity.Settings;

public class IdentitySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(IdentitySettings.ExternalLogin.Google.Enabled, "false"),
            new SettingDefinition(IdentitySettings.ExternalLogin.Google.ClientId),
            new SettingDefinition(IdentitySettings.ExternalLogin.Google.ClientSecret, isEncrypted: true),
            new SettingDefinition(IdentitySettings.ExternalLogin.EntraId.Enabled, "false"),
            new SettingDefinition(IdentitySettings.ExternalLogin.EntraId.TenantId),
            new SettingDefinition(IdentitySettings.ExternalLogin.EntraId.ClientId),
            new SettingDefinition(IdentitySettings.ExternalLogin.EntraId.ClientSecret, isEncrypted: true)
        );
    }
}
