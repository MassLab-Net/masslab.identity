using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MassLab.Identity.Permissions;
using MassLab.Identity.Settings;
using MassLab.Identity.Web.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Volo.Abp.SettingManagement;

namespace MassLab.Identity.Web.Pages.Administration;

[Authorize(IdentityPermissions.ExternalLoginProvidersManage)]
public class LoginProvidersModel : IdentityPageModel
{
    private readonly ISettingManager _settingManager;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public GoogleProviderInput Google { get; set; } = new();

    [BindProperty]
    public EntraIdProviderInput EntraId { get; set; } = new();

    public string GoogleCallbackUrl => BuildCallbackUrl("/signin-google");

    public string EntraIdCallbackUrl => BuildCallbackUrl(ExternalLoginProviderSchemes.EntraIdCallbackPath);

    public LoginProvidersModel(ISettingManager settingManager, IConfiguration configuration)
    {
        _settingManager = settingManager;
        _configuration = configuration;
    }

    public async Task OnGetAsync()
    {
        Google.Enabled = await GetBoolAsync(IdentitySettings.ExternalLogin.Google.Enabled);
        Google.ClientId = await _settingManager.GetOrNullForCurrentTenantAsync(IdentitySettings.ExternalLogin.Google.ClientId);

        EntraId.Enabled = await GetBoolAsync(IdentitySettings.ExternalLogin.EntraId.Enabled);
        EntraId.TenantId = await _settingManager.GetOrNullForCurrentTenantAsync(IdentitySettings.ExternalLogin.EntraId.TenantId);
        EntraId.ClientId = await _settingManager.GetOrNullForCurrentTenantAsync(IdentitySettings.ExternalLogin.EntraId.ClientId);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await SaveAsync(IdentitySettings.ExternalLogin.Google.Enabled, Google.Enabled.ToString());
        await SaveAsync(IdentitySettings.ExternalLogin.Google.ClientId, Google.ClientId);
        await SaveSecretAsync(IdentitySettings.ExternalLogin.Google.ClientSecret, Google.ClientSecret);

        await SaveAsync(IdentitySettings.ExternalLogin.EntraId.Enabled, EntraId.Enabled.ToString());
        await SaveAsync(IdentitySettings.ExternalLogin.EntraId.TenantId, EntraId.TenantId);
        await SaveAsync(IdentitySettings.ExternalLogin.EntraId.ClientId, EntraId.ClientId);
        await SaveSecretAsync(IdentitySettings.ExternalLogin.EntraId.ClientSecret, EntraId.ClientSecret);

        Alerts.Success(L["LoginProviders:Saved"]);
        return RedirectToPage();
    }

    private async Task<bool> GetBoolAsync(string name)
    {
        var value = await _settingManager.GetOrNullForCurrentTenantAsync(name);
        return bool.TryParse(value, out var enabled) && enabled;
    }

    private Task SaveAsync(string name, string? value)
    {
        return _settingManager.SetForCurrentTenantAsync(name, value?.Trim());
    }

    private async Task SaveSecretAsync(string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            await _settingManager.SetForCurrentTenantAsync(name, value.Trim());
        }
    }

    private string BuildCallbackUrl(string path)
    {
        var selfUrl = _configuration["App:SelfUrl"]?.TrimEnd('/') ?? string.Empty;
        return selfUrl + path;
    }

    public class GoogleProviderInput
    {
        public bool Enabled { get; set; }

        [Display(Name = "LoginProviders:ClientId")]
        public string? ClientId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "LoginProviders:ClientSecret")]
        public string? ClientSecret { get; set; }
    }

    public class EntraIdProviderInput
    {
        public bool Enabled { get; set; }

        [Display(Name = "LoginProviders:TenantId")]
        public string? TenantId { get; set; }

        [Display(Name = "LoginProviders:ClientId")]
        public string? ClientId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "LoginProviders:ClientSecret")]
        public string? ClientSecret { get; set; }
    }
}
