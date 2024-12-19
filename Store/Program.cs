using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("adminRole", policy =>
            policy.RequireRole("admin"));

        options.AddPolicy("contentManagerRole", policy =>
            policy.RequireRole("admin", "contentManager"));

        options.AddPolicy("editorRole", policy =>
            policy.RequireRole("admin", "contentManager", "editor"));
    });

    builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer("https://localhost:7229/");
        //options.AddAudiences("https://oauth.pstmn.io/");
        options.UseAspNetCore();  // Enable local token validation
        options.UseSystemNetHttp(); // For introspection or fetching metadata

        //options.UseIntrospection()
        //       .SetClientId("postman_client")
        //       .SetClientSecret("49D8E3A5-6586-428E-9D92-060CB692C876");
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

