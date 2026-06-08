using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace MassLab.Identity.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web host.");
            Log.Information("Creating web application builder.");
            var builder = WebApplication.CreateBuilder(args);
            Log.Information("Web application builder created.");
            Log.Information("Configuring host.");
            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .WriteTo.Async(c => c.AbpStudio(services));
                });
            Log.Information("Host configured.");
            Log.Information("Configuring ABP application.");
            await builder.AddApplicationAsync<IdentityWebModule>();
            Log.Information("ABP application configured.");
            Log.Information("Building web application.");
            var app = builder.Build();
            Log.Information("Initializing web application.");
            await app.InitializeApplicationAsync();
            Log.Information("Web application initialized.");
            Log.Information("Running web application.");
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
