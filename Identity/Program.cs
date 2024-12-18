using Identity;
using Identity.Configurations;
using Identity.Extentions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

const string CorsPolicy = "DefaultCorsPolicy";

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllersWithViews();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.ConfigureServices(builder.Configuration);
    builder.Services.ConfigureIdentity(builder.Configuration);
    builder.Services.ConfigureOpenIdDict();
    builder.Services.ConfigureCors(builder.Configuration, CorsPolicy);

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/account/login";
            })
            .AddJwtBearer(options =>
            {
                var identityOptions = builder.Configuration.GetSection(IdentityOptions.Key);

                var authorityUrl =
                    identityOptions.GetValue<string>(nameof(IdentityOptions.Authority));

                options.Authority = authorityUrl;

                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

    builder.Services.AddAuthorization();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    await DataSeeder.SeedAsync(app.Services);

    app.UseHttpsRedirection();
    app.UseCors(CorsPolicy);
    app.UseRouting();
    app.UseStaticFiles();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultControllerRoute();

    app.Run();
}
