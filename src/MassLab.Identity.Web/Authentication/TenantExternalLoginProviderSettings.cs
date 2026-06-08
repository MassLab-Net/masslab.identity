using System;
using System.Threading.Tasks;
using MassLab.Identity.Settings;
using Volo.Abp.Settings;

namespace MassLab.Identity.Web.Authentication;

public class TenantExternalLoginProviderSettings
{
    private readonly ISettingProvider _settingProvider;

    public TenantExternalLoginProviderSettings(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
    }

    public async Task<GoogleExternalLoginSettings> GetGoogleAsync()
    {
        return new GoogleExternalLoginSettings(
            await GetBoolAsync(IdentitySettings.ExternalLogin.Google.Enabled),
            await GetTrimmedAsync(IdentitySettings.ExternalLogin.Google.ClientId),
            await GetTrimmedAsync(IdentitySettings.ExternalLogin.Google.ClientSecret)
        );
    }

    public async Task<EntraIdExternalLoginSettings> GetEntraIdAsync()
    {
        var tenantId = await GetTrimmedAsync(IdentitySettings.ExternalLogin.EntraId.TenantId);
        return new EntraIdExternalLoginSettings(
            await GetBoolAsync(IdentitySettings.ExternalLogin.EntraId.Enabled),
            tenantId,
            await GetTrimmedAsync(IdentitySettings.ExternalLogin.EntraId.ClientId),
            await GetTrimmedAsync(IdentitySettings.ExternalLogin.EntraId.ClientSecret),
            BuildEntraAuthority(tenantId)
        );
    }

    private async Task<bool> GetBoolAsync(string name)
    {
        var value = await _settingProvider.GetOrNullAsync(name);
        return bool.TryParse(value, out var result) && result;
    }

    private async Task<string?> GetTrimmedAsync(string name)
    {
        var value = await _settingProvider.GetOrNullAsync(name);
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string? BuildEntraAuthority(string? tenantId)
    {
        return string.IsNullOrWhiteSpace(tenantId)
            ? null
            : $"https://login.microsoftonline.com/{Uri.EscapeDataString(tenantId)}/v2.0";
    }
}

public record GoogleExternalLoginSettings(bool Enabled, string? ClientId, string? ClientSecret)
{
    public bool IsComplete => Enabled &&
                              !string.IsNullOrWhiteSpace(ClientId) &&
                              !string.IsNullOrWhiteSpace(ClientSecret);
}

public record EntraIdExternalLoginSettings(
    bool Enabled,
    string? TenantId,
    string? ClientId,
    string? ClientSecret,
    string? Authority)
{
    public bool IsComplete => Enabled &&
                              !string.IsNullOrWhiteSpace(TenantId) &&
                              !string.IsNullOrWhiteSpace(ClientId) &&
                              !string.IsNullOrWhiteSpace(ClientSecret) &&
                              !string.IsNullOrWhiteSpace(Authority);
}
