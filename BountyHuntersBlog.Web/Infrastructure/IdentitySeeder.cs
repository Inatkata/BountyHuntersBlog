using BountyHuntersBlog.Data.Models;
using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Roles
        foreach (var role in new[] { "Admin", "User" })
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        // Admin
        var adminEmail = "admin@site.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = "admin", Email = adminEmail, DisplayName = "Admin" };
            await userManager.CreateAsync(admin, "Admin!123"); // TODO: move to secrets
        }
        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");

        // Demo user
        var userEmail = "demo@site.local";
        var demo = await userManager.FindByEmailAsync(userEmail);
        if (demo == null)
        {
            demo = new ApplicationUser { UserName = "demo", Email = userEmail, DisplayName = "Demo User" };
            await userManager.CreateAsync(demo, "User!123"); // TODO: move to secrets
        }
        if (!await userManager.IsInRoleAsync(demo, "User"))
            await userManager.AddToRoleAsync(demo, "User");
    }
}