using Identity.Data;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants.Permissions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.EntityFrameworkCore;
using Identity.Configurations;

namespace Identity;

public class SeedOrganizations
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        var clientSettings = configuration.GetSection("Clients").Get<List<ClientOptions>>();

        foreach (var clientSetting in clientSettings)
        {
            var client = await manager.FindByClientIdAsync(clientSetting.Id);
            var clientDescriptor = GetClientDescriptor(clientSetting);

            if (client is null)
            {
                await manager.CreateAsync(clientDescriptor);
            }
            else
            {
                if (clientDescriptor.ClientSecret is not null)
                {
                    var hasher = new PasswordHasher<object>();
                    var hashedSecret = hasher.HashPassword(clientDescriptor.ClientId!, clientDescriptor.ClientSecret);

                    clientDescriptor.ClientSecret = hashedSecret;
                }

                await manager.PopulateAsync(client, clientDescriptor);

                await manager.UpdateAsync(client, clientDescriptor);
            }
        }
    }

    private static OpenIddictApplicationDescriptor GetClientDescriptor(ClientOptions clientSetting)
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientSetting.Id,
            DisplayName = clientSetting.Name,
            ClientType = clientSetting.RequireClientSecret ? ClientTypes.Confidential : ClientTypes.Public,
            ClientSecret = clientSetting.RequireClientSecret ? clientSetting.Secret : null,
        };

        if (clientSetting.RedirectUris != null)
        {
            foreach (var redirectUri in clientSetting.RedirectUris.Select(uri => new Uri(uri)))
            {
                descriptor.RedirectUris.Add(redirectUri);
            }
        }

        if (clientSetting.PostLogoutRedirectUris != null)
        {
            foreach (var logoutRedirectUri in clientSetting.PostLogoutRedirectUris.Select(uri => new Uri(uri)))
            {
                descriptor.PostLogoutRedirectUris.Add(logoutRedirectUri);
            }
        }

        descriptor.Permissions.Add(Endpoints.Authorization);
        descriptor.Permissions.Add(Endpoints.Token);
        descriptor.Permissions.Add(Endpoints.EndSession);

        descriptor.Permissions.Add(Permissions.ResponseTypes.Token);
        descriptor.Permissions.Add(Permissions.ResponseTypes.IdToken);
        descriptor.Permissions.Add(Permissions.ResponseTypes.IdTokenToken);
        descriptor.Permissions.Add(Permissions.ResponseTypes.Code);
        descriptor.Permissions.Add(Permissions.ResponseTypes.CodeIdToken);
        descriptor.Permissions.Add(Permissions.ResponseTypes.CodeIdTokenToken);
        descriptor.Permissions.Add(Permissions.ResponseTypes.CodeToken);

        descriptor.Permissions.Add(Permissions.Scopes.Email);
        descriptor.Permissions.Add(Permissions.Scopes.Profile);
        descriptor.Permissions.Add(Permissions.Scopes.Roles);

        // Configure Token Lifetime for the Client
        descriptor.Settings.Add(Settings.TokenLifetimes.AccessToken, TimeSpan.FromSeconds(clientSetting.TokenLifetime).ToString());

        // Add Grant Types
        foreach (var grantType in clientSetting.AllowedGrantTypes)
        {
            descriptor.Permissions.Add(Prefixes.GrantType + grantType);
        }

        // Add Scopes
        foreach (var clientScope in clientSetting.AllowedScopes)
        {
            descriptor.Permissions.Add(Prefixes.Scope + clientScope);
        }

        // Add PKCE requirement if specified
        if (clientSetting.RequirePkce)
        {
            descriptor.Requirements.Add(Requirements.Features.ProofKeyForCodeExchange);
        }

        return descriptor;
    }
}

