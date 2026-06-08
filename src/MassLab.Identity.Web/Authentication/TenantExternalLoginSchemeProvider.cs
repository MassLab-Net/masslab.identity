using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MassLab.Identity.Web.Authentication;

public class TenantExternalLoginSchemeProvider : AuthenticationSchemeProvider
{
    private static readonly HashSet<string> TenantProviderSchemes =
    [
        ExternalLoginProviderSchemes.Google,
        ExternalLoginProviderSchemes.EntraId
    ];

    private readonly IServiceProvider _serviceProvider;

    public TenantExternalLoginSchemeProvider(
        IOptions<AuthenticationOptions> options,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<AuthenticationScheme?> GetSchemeAsync(string name)
    {
        var scheme = await base.GetSchemeAsync(name);
        if (scheme == null || !TenantProviderSchemes.Contains(name))
        {
            return scheme;
        }

        return await IsEnabledAndCompleteAsync(name) ? scheme : null;
    }

    public override async Task<IEnumerable<AuthenticationScheme>> GetAllSchemesAsync()
    {
        var schemes = (await base.GetAllSchemesAsync()).ToList();

        foreach (var schemeName in TenantProviderSchemes)
        {
            if (!await IsEnabledAndCompleteAsync(schemeName))
            {
                schemes.RemoveAll(s => s.Name == schemeName);
            }
        }

        return schemes;
    }

    private async Task<bool> IsEnabledAndCompleteAsync(string scheme)
    {
        using var scope = _serviceProvider.CreateScope();
        var settings = scope.ServiceProvider.GetRequiredService<TenantExternalLoginProviderSettings>();

        return scheme switch
        {
            ExternalLoginProviderSchemes.Google => (await settings.GetGoogleAsync()).IsComplete,
            ExternalLoginProviderSchemes.EntraId => (await settings.GetEntraIdAsync()).IsComplete,
            _ => true
        };
    }
}
