using Microsoft.AspNetCore.Identity;

namespace Identity.Data.Seed;

public class SeedUsers
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var adminRole = await roleManager.FindByNameAsync("admin");

        if (adminRole is null)
        {
            adminRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin"
            };

            await roleManager.CreateAsync(adminRole);
        }

        var adminUser = await userManager.FindByNameAsync("Admin");

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NameAr = "Admin",
                NameFr = "Admin",
                Email = "admin@email.com",
                UserName = "admin",
                CreatedAt = DateTimeOffset.Now
            };

            await userManager.CreateAsync(adminUser);
            await userManager.AddPasswordAsync(adminUser, "P@ssw0rd");
            await userManager.AddToRoleAsync(adminUser, adminRole.Name!);
        }
    }
}
