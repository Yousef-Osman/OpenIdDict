using Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityOptions = Identity.Configurations.IdentityOptions;
using Identity.Configurations;
using Identity.Interfaces;
using Identity.Services;

namespace Identity;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetSection(IdentityOptions.Key));
        services.Configure<SecurityOptions>(configuration.GetSection(SecurityOptions.Key));
        services.AddScoped<IAccountService, LocalAccountService>();
    }

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration, string corsPolicy)
    {
        var securityOptions = configuration.GetSection(SecurityOptions.Key);

        var allowedOrigins = securityOptions.GetValue<string>(nameof(SecurityOptions.AllowedOrigins)).Split(';');

        services.AddCors(o => o.AddPolicy(corsPolicy, builder =>
        {
            builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }));

        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(securityOptions.GetValue<int>(nameof(SecurityOptions.HstsMaxAgeInDays)));
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(IdentityOptions.ConnectionStringKey);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseOpenIddict();
        });

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(connectionString)));

    }
    public static void ConfigureOpenIdDict(this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
            })
            .AddServer(options =>
            {
                options
                    .AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow()
                    .AllowPasswordFlow()
                    .AllowRefreshTokenFlow()
                    .RequireProofKeyForCodeExchange();

                options
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserInfoEndpointUris("/connect/userinfo")
                    .SetEndSessionEndpointUris("/connect/endsession"); //logout

                options
                    .AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey()
                .DisableAccessTokenEncryption(); //disabled temp

                options.RegisterScopes("offline_access", "user_identity", "profile");

                options
                    .AddDevelopmentSigningCertificate()
                    .AddDevelopmentEncryptionCertificate();

                options
                    .UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseSystemNetHttp();
            });
    }
}
