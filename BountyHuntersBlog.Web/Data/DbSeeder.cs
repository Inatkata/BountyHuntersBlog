using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            // 1. Създай ролите, ако не съществуват
            string[] roles = { "Admin", "ApplicationUser" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Дефинирай SuperAdmin потребител
            var adminEmail = "admin@bountyApplicationUsers.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    DisplayName = "Super Admin",
                    EmailConfirmed = true
                };

                // Може да смениш паролата
                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddToRoleAsync(adminUser, "User");
                }
                else
                {
                    // Логване на грешките
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                    }
                }
            }
        }
    }
}