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

        var contentManagerRole = await roleManager.FindByNameAsync("contentManager");

        if (contentManagerRole is null)
        {
            contentManagerRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "contentManager"
            };

            await roleManager.CreateAsync(contentManagerRole);
        }

        var editorRole = await roleManager.FindByNameAsync("editor");

        if (editorRole is null)
        {
            editorRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "editor"
            };

            await roleManager.CreateAsync(editorRole);
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

        var contentManagerUser = await userManager.FindByNameAsync("siteContentManager");

        if (contentManagerUser is null)
        {
            contentManagerUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Site Content Manager",
                NameAr = "Site Content Manager",
                NameFr = "Site Content Manager",
                Email = "siteContentManager@email.com",
                UserName = "siteContentManager",
                CreatedAt = DateTimeOffset.Now
            };

            await userManager.CreateAsync(contentManagerUser);
            await userManager.AddPasswordAsync(contentManagerUser, "P@ssw0rd");
            await userManager.AddToRoleAsync(contentManagerUser, contentManagerRole.Name!);
        }

        var editorUser = await userManager.FindByNameAsync("siteEditor");

        if (editorUser is null)
        {
            editorUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Site Editor",
                NameAr = "Site Editor",
                NameFr = "Site Editor",
                Email = "siteEditor@email.com",
                UserName = "siteEditor",
                CreatedAt = DateTimeOffset.Now
            };

            await userManager.CreateAsync(editorUser);
            await userManager.AddPasswordAsync(editorUser, "P@ssw0rd");
            await userManager.AddToRoleAsync(editorUser, editorRole.Name!);
        }
    }
}
