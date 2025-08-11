using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Web.Infrastructure
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BountyHuntersDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Categories
            var cats = new[] { "Bounty Missions", "Exploration", "Rescue" };
            foreach (var name in cats)
            {
                if (!await db.Categories.AnyAsync(c => c.Name == name))
                    db.Categories.Add(new Category { Name = name });
            }
            await db.SaveChangesAsync();

            // Tags
            var tags = new[] { "Dangerous", "Urgent", "Long-term" };
            foreach (var name in tags)
            {
                if (!await db.Tags.AnyAsync(t => t.Name == name))
                    db.Tags.Add(new Tag { Name = name });
            }
            await db.SaveChangesAsync();

            // Author = admin if exists, else first user
            var admins = await (from u in db.Users
                                join ur in db.UserRoles on u.Id equals ur.UserId
                                join r in db.Roles on ur.RoleId equals r.Id
                                where r.Name == "Admin"
                                select u).ToListAsync();
            var authorId = admins.FirstOrDefault()?.Id ?? await db.Users.Select(u => u.Id).FirstOrDefaultAsync();
            if (authorId == null) return; // no users yet

            // Resolve some ids
            var cat = await db.Categories.FirstAsync();
            var tagUrgent = await db.Tags.FirstAsync(t => t.Name == "Urgent");
            var tagDanger = await db.Tags.FirstAsync(t => t.Name == "Dangerous");

            // Missions (idempotent by Title)
            async Task AddMissionAsync(string title, string desc, int categoryId, params string[] tagNames)
            {
                if (await db.Missions.AnyAsync(m => m.Title == title)) return;

                var mission = new Mission
                {
                    Title = title,
                    Description = desc,
                    CategoryId = categoryId,
                    AuthorId = authorId
                };

                mission.MissionTags = new List<MissionTag>();
                foreach (var tn in tagNames.Distinct())
                {
                    var tg = await db.Tags.FirstAsync(t => t.Name == tn);
                    mission.MissionTags.Add(new MissionTag { TagId = tg.Id });
                }

                db.Missions.Add(mission);
                await db.SaveChangesAsync();
            }

            await AddMissionAsync(
                "Silent Run in Old Town",
                "Plant trackers on targets. Avoid cameras; exfil via north alley.",
                cat.Id,
                "Urgent", "Dangerous");

            await AddMissionAsync(
                "Locate Lost Drone",
                "Scan sector 7, retrieve downed UAV core.",
                cat.Id,
                "Long-term");
        }
    }
}
