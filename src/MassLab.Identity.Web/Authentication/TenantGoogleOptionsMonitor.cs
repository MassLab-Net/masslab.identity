using System;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace MassLab.Identity.Web.Authentication;

public class TenantGoogleOptionsMonitor : IOptionsMonitor<GoogleOptions>
{
    private readonly IOptionsFactory<GoogleOptions> _factory;
    private readonly IServiceProvider _serviceProvider;

    public TenantGoogleOptionsMonitor(
        IOptionsFactory<GoogleOptions> factory,
        IServiceProvider serviceProvider)
    {
        _factory = factory;
        _serviceProvider = serviceProvider;
    }

    public GoogleOptions CurrentValue => Get(Options.DefaultName);

    public GoogleOptions Get(string? name)
    {
        var options = _factory.Create(name ?? Options.DefaultName);

        using var scope = _serviceProvider.CreateScope();
        var settings = AsyncHelper.RunSync(() =>
            scope.ServiceProvider.GetRequiredService<TenantExternalLoginProviderSettings>().GetGoogleAsync());

        if (settings.IsComplete)
        {
            options.ClientId = settings.ClientId!;
            options.ClientSecret = settings.ClientSecret!;
        }

        return options;
    }

    public IDisposable? OnChange(Action<GoogleOptions, string?> listener)
    {
        return null;
    }
}
