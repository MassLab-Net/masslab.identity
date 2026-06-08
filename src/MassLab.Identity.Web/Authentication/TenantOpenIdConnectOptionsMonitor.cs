using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Volo.Abp.Threading;

namespace MassLab.Identity.Web.Authentication;

public class TenantOpenIdConnectOptionsMonitor : IOptionsMonitor<OpenIdConnectOptions>
{
    private readonly IOptionsFactory<OpenIdConnectOptions> _factory;
    private readonly IServiceProvider _serviceProvider;

    public TenantOpenIdConnectOptionsMonitor(
        IOptionsFactory<OpenIdConnectOptions> factory,
        IServiceProvider serviceProvider)
    {
        _factory = factory;
        _serviceProvider = serviceProvider;
    }

    public OpenIdConnectOptions CurrentValue => Get(Options.DefaultName);

    public OpenIdConnectOptions Get(string? name)
    {
        var options = _factory.Create(name ?? Options.DefaultName);
        if (name != ExternalLoginProviderSchemes.EntraId)
        {
            return options;
        }

        using var scope = _serviceProvider.CreateScope();
        var settings = AsyncHelper.RunSync(() =>
            scope.ServiceProvider.GetRequiredService<TenantExternalLoginProviderSettings>().GetEntraIdAsync());

        if (settings.IsComplete)
        {
            options.Authority = settings.Authority;
            options.MetadataAddress = $"{settings.Authority!.TrimEnd('/')}/.well-known/openid-configuration";
            options.ClientId = settings.ClientId!;
            options.ClientSecret = settings.ClientSecret!;
            options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                options.MetadataAddress,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever { RequireHttps = options.RequireHttpsMetadata });
        }

        return options;
    }

    public IDisposable? OnChange(Action<OpenIdConnectOptions, string?> listener)
    {
        return null;
    }
}
