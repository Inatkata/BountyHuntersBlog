using System.Security.Claims;
using BountyHuntersBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BountyHuntersBlog.Web.Infrastructure
{
    public static class IdentitySeeder
    {
        public const string RoleUser = "User";
        public const string RoleAdmin = "Administrator";

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var config = provider.GetRequiredService<IConfiguration>();

            // 1) Ensure roles
            await EnsureRoleAsync(roleManager, RoleUser);
            await EnsureRoleAsync(roleManager, RoleAdmin);

            // 2) Admin user (from config or defaults)
            var adminUserName = config["Seed:Admin:UserName"] ?? "admin";
            var adminEmail = config["Seed:Admin:Email"] ?? "admin@bountyhunters.local";
            var adminPass = config["Seed:Admin:Password"] ?? "Admin!234";

            var admin = await userManager.FindByNameAsync(adminUserName);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var createRes = await userManager.CreateAsync(admin, adminPass);
                if (!createRes.Succeeded)
                    throw new InvalidOperationException("Failed to create admin user: " + string.Join("; ", createRes.Errors.Select(e => e.Description)));
            }

            // 3) Ensure admin has roles
            var roles = await userManager.GetRolesAsync(admin);
            if (!roles.Contains(RoleAdmin))
                await userManager.AddToRoleAsync(admin, RoleAdmin);
            if (!roles.Contains(RoleUser))
                await userManager.AddToRoleAsync(admin, RoleUser);

            // 4) (optional) admin claims
            await EnsureClaimAsync(userManager, admin, new Claim("display_name", "System Admin"));
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var res = await roleManager.CreateAsync(new IdentityRole(role));
                if (!res.Succeeded)
                    throw new InvalidOperationException($"Failed to create role '{role}': " + string.Join("; ", res.Errors.Select(e => e.Description)));
            }
        }

        private static async Task EnsureClaimAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, Claim claim)
        {
            var claims = await userManager.GetClaimsAsync(user);
            if (!claims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                await userManager.AddClaimAsync(user, claim);
        }
    }
}
