using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Hunter>>();

            string[] roles = { "Admin", "Hunter" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Създаване на Admin потребител, ако няма такъв
            var adminEmail = "admin@bounty.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                var newAdmin = new Hunter
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    DisplayName = "Administrator"
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}