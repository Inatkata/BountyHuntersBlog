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

            await db.Database.MigrateAsync();

            // users to attach content to
            var admin = await userManager.FindByEmailAsync("admin@site.local");
            var demo = await userManager.FindByEmailAsync("demo@site.local");
            var userIds = new[] { admin?.Id, demo?.Id }
                .Where(x => !string.IsNullOrEmpty(x))
                .Cast<string>()
                .ToList();
            if (userIds.Count == 0) return; // no users yet

            // ---- Categories (5) ----
            var categories = new[]
            {
                "Bounty Missions","Exploration","Rescue","Recon","Intel"
            };
            foreach (var name in categories)
                if (!await db.Categories.AnyAsync(c => c.Name == name))
                    db.Categories.Add(new Category { Name = name });
            await db.SaveChangesAsync();

            // ---- Tags (8) ----
            var tags = new[]
            {
                "Urgent","Dangerous","Stealth","NightOp","HighRisk","Recon","Long-term","Urban"
            };
            foreach (var name in tags)
                if (!await db.Tags.AnyAsync(t => t.Name == name))
                    db.Tags.Add(new Tag { Name = name });
            await db.SaveChangesAsync();

            // helpers
            async Task<int> CatId(string name) => (await db.Categories.FirstAsync(c => c.Name == name)).Id;
            async Task<int> TagId(string name) => (await db.Tags.FirstAsync(t => t.Name == name)).Id;

            // ---- Missions (10) + comments + likes (идемпотентно по Title) ----
            var missions = new (string Title, string Desc, string Cat, string[] TagNames)[]
            {
                ("Silent Run in Old Town", "Plant trackers on targets. Avoid cameras; exfil via north alley.", "Bounty Missions", new[]{"Urgent","Dangerous","Urban"}),
                ("Locate Lost Drone", "Scan sector 7 and retrieve downed UAV core.", "Recon", new[]{"Recon","Long-term"}),
                ("Extract Hostage Zeta", "Infiltrate compound and extract VIP. Use night vision.", "Rescue", new[]{"NightOp","Urgent"}),
                ("Map Sewer Network", "Create updated map for smuggling counter-ops.", "Intel", new[]{"Long-term","Urban"}),
                ("Trail Smuggler Route", "Shadow convoy without engaging.", "Exploration", new[]{"Stealth","Recon"}),
                ("Disable Signal Tower", "EMP charge on tower, minimize collateral.", "Bounty Missions", new[]{"Dangerous","HighRisk"}),
                ("Recover Hard Drive", "Obtain drive from safehouse attic.", "Intel", new[]{"Stealth","Urban"}),
                ("Test Perimeter Drones", "Night patrol of zone C with drones.", "Recon", new[]{"NightOp","Long-term"}),
                ("Escort Medic Team", "Protect medics entering hot zone.", "Rescue", new[]{"Urgent","HighRisk"}),
                ("Survey Desert Ruins", "Capture imagery and mark hazards.", "Exploration", new[]{"Long-term","Recon"})
            };

            foreach (var m in missions)
            {
                if (await db.Missions.AnyAsync(x => x.Title == m.Title)) continue;

                var mission = new Mission
                {
                    Title = m.Title,
                    Description = m.Desc,
                    CategoryId = await CatId(m.Cat),
                    UserId = userIds[0],
                    IsCompleted = false,
                    CreatedOn = DateTime.UtcNow
                };

                mission.MissionTags = new List<MissionTag>();
                foreach (var tn in m.TagNames.Distinct())
                    mission.MissionTags.Add(new MissionTag { TagId = await TagId(tn) });

                db.Missions.Add(mission);
                await db.SaveChangesAsync();

                // comments (2..3)
                var c1 = new Comment { MissionId = mission.Id, UserId = userIds[0], Content = "Noted. Preparing gear.", CreatedOn = DateTime.UtcNow.AddMinutes(-30) };
                var c2 = new Comment { MissionId = mission.Id, UserId = userIds[^1], Content = "Copy. I will handle recon.", CreatedOn = DateTime.UtcNow.AddMinutes(-20) };
                db.Comments.AddRange(c1, c2);
                if (m.TagNames.Contains("HighRisk"))
                {
                    db.Comments.Add(new Comment { MissionId = mission.Id, UserId = userIds[0], Content = "High risk confirmed. Request backup.", CreatedOn = DateTime.UtcNow.AddMinutes(-10) });
                }
                await db.SaveChangesAsync();

                // likes (1..2)
                db.Likes.Add(new Like { MissionId = mission.Id, UserId = userIds[0] });
                if (userIds.Count > 1) db.Likes.Add(new Like { MissionId = mission.Id, UserId = userIds[1] });
                await db.SaveChangesAsync();
            }
        }
    }
}
