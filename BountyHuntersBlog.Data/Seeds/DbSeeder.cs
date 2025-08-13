using BountyHuntersBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BountyHuntersBlog.Data.Seeding
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = services.GetRequiredService<BountyHuntersDbContext>();

            // 1) Seed Roles
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2) Seed Admin User
            string adminEmail = "admin@bh.local";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    DisplayName = "Administrator",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin!234");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // 3) Seed Normal User
            string userEmail = "user@bh.local";
            var normalUser = await userManager.FindByEmailAsync(userEmail);
            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    UserName = "user",
                    Email = userEmail,
                    DisplayName = "HunterJoe",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(normalUser, "User!234");
                await userManager.AddToRoleAsync(normalUser, "User");
            }

            // 4) Seed Categories
            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category { Name = "Bounty", CreatedOn = DateTime.UtcNow },
                    new Category { Name = "Rescue", CreatedOn = DateTime.UtcNow },
                    new Category { Name = "Tracking", CreatedOn = DateTime.UtcNow }
                };
                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }

            // 5) Seed Tags
            if (!await dbContext.Tags.AnyAsync())
            {
                var tags = new[]
                {
                    new Tag { Name = "High Priority", CreatedOn = DateTime.UtcNow },
                    new Tag { Name = "Covert", CreatedOn = DateTime.UtcNow },
                    new Tag { Name = "Long Range", CreatedOn = DateTime.UtcNow },
                    new Tag { Name = "Urban", CreatedOn = DateTime.UtcNow },
                    new Tag { Name = "Night Ops", CreatedOn = DateTime.UtcNow }
                };
                await dbContext.Tags.AddRangeAsync(tags);
                await dbContext.SaveChangesAsync();
            }

            // 6) Seed Missions
            if (!await dbContext.Missions.AnyAsync())
            {
                var firstCategoryId = await dbContext.Categories.Select(c => c.Id).FirstAsync();
                var tagIds = await dbContext.Tags.Select(t => t.Id).Take(2).ToListAsync();

                var mission = new Mission
                {
                    Title = "Capture Rogue Droid",
                    Description = "Locate and capture the rogue droid last seen in sector 7.",
                    ImageUrl = "/images/missions/droid.jpg",
                    UserId = adminUser.Id,
                    CategoryId = firstCategoryId,
                    CreatedOn = DateTime.UtcNow,
                    IsCompleted = false
                };

                foreach (var tagId in tagIds)
                {
                    mission.MissionTags.Add(new MissionTag
                    {
                        TagId = tagId,
                        CreatedOn = DateTime.UtcNow
                    });
                }

                await dbContext.Missions.AddAsync(mission);
                await dbContext.SaveChangesAsync();

                // 7) Seed Comments
                var comment = new Comment
                {
                    Content = "This mission is dangerous, proceed with caution.",
                    UserId = normalUser.Id,
                    MissionId = mission.Id,
                    CreatedOn = DateTime.UtcNow
                };
                await dbContext.Comments.AddAsync(comment);
                await dbContext.SaveChangesAsync();

                // 8) Seed Likes
                var like = new Like
                {
                    UserId = normalUser.Id,
                    MissionId = mission.Id,
                    CreatedOn = DateTime.UtcNow
                };
                await dbContext.Likes.AddAsync(like);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
