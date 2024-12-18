using Identity.Data.Seed;

namespace Identity.Extentions;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            await SeedOrganizations.InitializeAsync(scope.ServiceProvider, configuration);
            await SeedUsers.InitializeAsync(scope.ServiceProvider, configuration);
        }
    }
}
