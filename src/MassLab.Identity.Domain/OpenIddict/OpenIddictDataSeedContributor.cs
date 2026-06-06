using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Uow;

namespace MassLab.Identity.OpenIddict;

/* Creates initial data that is needed to property run the application
 * and make client-to-server communication possible.
 */
public class OpenIddictDataSeedContributor : OpenIddictDataSeedContributorBase, IDataSeedContributor, ITransientDependency
{
    public OpenIddictDataSeedContributor(
        IConfiguration configuration,
        IOpenIddictApplicationRepository openIddictApplicationRepository,
        IAbpApplicationManager applicationManager,
        IOpenIddictScopeRepository openIddictScopeRepository,
        IOpenIddictScopeManager scopeManager)
        : base(configuration, openIddictApplicationRepository, applicationManager, openIddictScopeRepository, scopeManager)
    {
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateScopesAsync();
        await CreateApplicationsAsync();
    }

    private async Task CreateScopesAsync()
    {
        await CreateScopesAsync(new OpenIddictScopeDescriptor 
        {
            Name = "Identity", 
            DisplayName = "Identity API", 
            Resources = { "Identity" }
        });
    }

    private async Task CreateApplicationsAsync()
    {
        var commonScopes = new List<string> {
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles,
            "Identity"
        };

        var configurationSection = Configuration.GetSection("OpenIddict:Applications");


        // Console Test / Angular Client
        
        var appClientId = configurationSection["Identity_App:ClientId"];
        if (!appClientId.IsNullOrWhiteSpace())
        {
            var appClientRootUrl = configurationSection["Identity_App:RootUrl"]?.TrimEnd('/');
            if (!appClientRootUrl.IsNullOrWhiteSpace())
            {
                await CreateOrUpdateApplicationAsync(
                    applicationType: OpenIddictConstants.ApplicationTypes.Web,
                    name: appClientId!,
                    type: OpenIddictConstants.ClientTypes.Public,
                    consentType: OpenIddictConstants.ConsentTypes.Implicit,
                    displayName: "Console Test / Angular Application",
                    secret: null,
                    grantTypes: new List<string> {
                        OpenIddictConstants.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.GrantTypes.Password,
                        OpenIddictConstants.GrantTypes.ClientCredentials,
                        OpenIddictConstants.GrantTypes.RefreshToken,
                        "LinkLogin",
                        "Impersonation"
                    },
                    scopes: commonScopes,
                    redirectUris: new List<string> { appClientRootUrl },
                    postLogoutRedirectUris: new List<string> { appClientRootUrl },
                    clientUri: appClientRootUrl,
                    logoUri: "/images/clients/angular.svg"
                );
            }
        }

        await CreateSpaApplicationAsync(
            configurationSection,
            "MassLab_IdentityAdmin",
            "MassLab Identity Admin SPA",
            commonScopes);

        await CreateSpaApplicationAsync(
            configurationSection,
            "MassLab_ShopWeb",
            "MassLab Shop Web",
            commonScopes);

        
        




        // Swagger Client
        var swaggerClientId = configurationSection["Identity_Swagger:ClientId"];
        if (!swaggerClientId.IsNullOrWhiteSpace())
        {
            var swaggerRootUrl = configurationSection["Identity_Swagger:RootUrl"]?.TrimEnd('/');
            if (swaggerRootUrl.IsNullOrWhiteSpace())
            {
                return;
            }

            await CreateOrUpdateApplicationAsync(
                applicationType: OpenIddictConstants.ApplicationTypes.Web,
                name: swaggerClientId!,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Swagger Application",
                secret: null,
                grantTypes: new List<string> { OpenIddictConstants.GrantTypes.AuthorizationCode, },
                scopes: commonScopes,
                redirectUris: new List<string> { $"{swaggerRootUrl}/swagger/oauth2-redirect.html" },
                clientUri: swaggerRootUrl.EnsureEndsWith('/') + "swagger",
                logoUri: "/images/clients/swagger.svg"
            );
        }


    }

    private async Task CreateSpaApplicationAsync(
        IConfigurationSection configurationSection,
        string sectionName,
        string displayName,
        List<string> scopes)
    {
        var clientId = configurationSection[$"{sectionName}:ClientId"];
        if (clientId.IsNullOrWhiteSpace())
        {
            return;
        }

        var rootUrl = configurationSection[$"{sectionName}:RootUrl"]?.TrimEnd('/');
        if (rootUrl.IsNullOrWhiteSpace())
        {
            return;
        }

        await CreateOrUpdateApplicationAsync(
            applicationType: OpenIddictConstants.ApplicationTypes.Web,
            name: clientId!,
            type: OpenIddictConstants.ClientTypes.Public,
            consentType: OpenIddictConstants.ConsentTypes.Implicit,
            displayName: displayName,
            secret: null,
            grantTypes: new List<string>
            {
                OpenIddictConstants.GrantTypes.AuthorizationCode,
                OpenIddictConstants.GrantTypes.RefreshToken
            },
            scopes: scopes,
            redirectUris: new List<string>
            {
                $"{rootUrl}/auth/callback",
                $"{rootUrl}/signin-oidc"
            },
            postLogoutRedirectUris: new List<string>
            {
                rootUrl
            },
            clientUri: rootUrl,
            logoUri: null
        );
    }
}
