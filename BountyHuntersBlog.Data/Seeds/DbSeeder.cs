using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data.Seeds
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var db = sp.GetRequiredService<BountyHuntersDbContext>();
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

            // 1) Roles
            foreach (var role in new[] { "Admin", "User" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2) Admin user (с DisplayName)
            const string adminUserName = "admin";
            const string adminEmail = "admin@bountyhunters.local";
            const string adminPass = "Admin!234"; // смени след първи старт

            var admin = await userManager.FindByNameAsync(adminUserName);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    DisplayName = "Administrator"
                };

                var create = await userManager.CreateAsync(admin, adminPass);
                if (!create.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", create.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(admin, "Admin");
                // Няма нужда от допълнителен Role claim – ролята е достатъчна
            }

            // 3) Categories (idempotent по име, case-insensitive)
            if (!await db.Categories.AnyAsync())
            {
                db.Categories.AddRange(
                    new Category { Name = "Stealth Ops" },
                    new Category { Name = "Assault" },
                    new Category { Name = "Recon" }
                );
                await db.SaveChangesAsync();
            }

            // 4) Tags
            if (!await db.Tags.AnyAsync())
            {
                db.Tags.AddRange(
                    new Tag { Name = "urgent" },
                    new Tag { Name = "stealth" },
                    new Tag { Name = "longterm" }
                );
                await db.SaveChangesAsync();
            }

            // 5) Missions (CreatedOn идва от DB default)
            if (!await db.Missions.AnyAsync())
            {
                var anyCategory = await db.Categories.OrderBy(c => c.Id).FirstAsync();
                var adminId = admin.Id;

                var m1 = new Mission
                {
                    Title = "Silent Run in Old Town",
                    Description = "Plant trackers on targets. Avoid cameras; exfil via north alley.",
                    UserId = adminId,
                    CategoryId = anyCategory.Id
                };
                var m2 = new Mission
                {
                    Title = "Convoy Intercept",
                    Description = "Stop and scan cargo. Use EMP drone; minimal collateral.",
                    UserId = adminId,
                    CategoryId = anyCategory.Id
                };

                db.Missions.AddRange(m1, m2);
                await db.SaveChangesAsync();

                // 6) MissionTags
                var tagStealth = await db.Tags.FirstOrDefaultAsync(t => t.Name == "stealth");
                var tagUrgent = await db.Tags.FirstOrDefaultAsync(t => t.Name == "urgent");
                if (tagStealth != null) db.MissionTags.Add(new MissionTag { MissionId = m1.Id, TagId = tagStealth.Id });
                if (tagUrgent != null) db.MissionTags.Add(new MissionTag { MissionId = m2.Id, TagId = tagUrgent.Id });
                await db.SaveChangesAsync();
            }

            // 7) Sample comment + like
            if (!await db.Comments.AnyAsync())
            {
                var mission = await db.Missions.FirstAsync();
                db.Comments.Add(new Comment
                {
                    MissionId = mission.Id,
                    UserId = admin.Id,
                    Content = "Initial briefing acknowledged."
                    // CreatedOn -> DB default
                });
                await db.SaveChangesAsync();
            }

            if (!await db.Likes.AnyAsync())
            {
                var mission = await db.Missions.FirstAsync();
                db.Likes.Add(new Like
                {
                    MissionId = mission.Id,
                    UserId = admin.Id
                    // CreatedOn -> DB default; XOR правило спазено (CommentId null)
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
