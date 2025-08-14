using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BountyHuntersBlog.Web.Infrastructure
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BountyHuntersDbContext>();

            // Ensure DB
            await db.Database.EnsureCreatedAsync();

            // === Categories ===
            var categories = new[]
            {
                new Category { Name = "Bounty" },
                new Category { Name = "Rescue" },
                new Category { Name = "Investigation" },
                new Category { Name = "Escort" }
            };
            foreach (var c in categories)
            {
                if (!await db.Categories.AnyAsync(x => x.Name == c.Name))
                    db.Categories.Add(c);
            }
            await db.SaveChangesAsync();

            // === Tags ===
            var tags = new[]
            {
                new Tag { Name = "HighRisk" },
                new Tag { Name = "Stealth" },
                new Tag { Name = "Urban" },
                new Tag { Name = "Wilderness" },
                new Tag { Name = "MostWanted" }
            };
            foreach (var t in tags)
            {
                if (!await db.Tags.AnyAsync(x => x.Name == t.Name))
                    db.Tags.Add(t);
            }
            await db.SaveChangesAsync();

            // Resolve IDs
            var catMap = await db.Categories.ToDictionaryAsync(c => c.Name, c => c.Id);
            var tagMap = await db.Tags.ToDictionaryAsync(t => t.Name, t => t.Id);

            // === Default author (admin) ===
            var admin = await db.Users.OrderBy(u => u.Id).FirstOrDefaultAsync(u => u.UserName == "admin");
            var authorId = admin?.Id ?? await db.Users.Select(u => u.Id).FirstOrDefaultAsync();

            // === Missions (only if none) ===
            if (!await db.Missions.AnyAsync())
            {
                var sample = new[]
                {
                    new Mission
                    {
                        Title = "Capture the Desert Outlaw",
                        Description = "Locate and capture the outlaw roaming the desert canyons.",
                        ImageUrl = "https://images.unsplash.com/photo-1519681393784-d120267933ba?q=80&w=1200",
                        CategoryId = catMap["Bounty"],
                        UserId = authorId!
                    },
                    new Mission
                    {
                        Title = "Rescue at Frozen Peak",
                        Description = "A team is stranded at the peak. Bring them back safely.",
                        ImageUrl = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?q=80&w=1200",
                        CategoryId = catMap["Rescue"],
                        UserId = authorId!
                    },
                    new Mission
                    {
                        Title = "Undercover in Night City",
                        Description = "Go undercover and gather intel on gang activity downtown.",
                        ImageUrl = "https://images.unsplash.com/photo-1496307042754-b4aa456c4a2d?q=80&w=1200",
                        CategoryId = catMap["Investigation"],
                        UserId = authorId!
                    },
                    new Mission
                    {
                        Title = "Convoy Escort South",
                        Description = "Escort a precious cargo through hostile territory.",
                        ImageUrl = "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?q=80&w=1200",
                        CategoryId = catMap["Escort"],
                        UserId = authorId!
                    }
                };

                db.Missions.AddRange(sample);
                await db.SaveChangesAsync();

                // Attach tags (MissionTag)
                var allMissions = await db.Missions.ToListAsync();
                foreach (var m in allMissions)
                {
                    var attach = new List<int>();
                    if (m.Title.Contains("Outlaw")) attach.Add(tagMap["MostWanted"]);
                    if (m.Title.Contains("Rescue")) attach.Add(tagMap["Wilderness"]);
                    if (m.Title.Contains("Undercover")) attach.Add(tagMap["Urban"]);
                    if (m.Title.Contains("Escort")) attach.Add(tagMap["HighRisk"]);

                    foreach (var tagId in attach.Distinct())
                    {
                        if (!await db.MissionTags.AnyAsync(mt => mt.MissionId == m.Id && mt.TagId == tagId))
                            db.MissionTags.Add(new MissionTag { MissionId = m.Id, TagId = tagId });
                    }
                }

                await db.SaveChangesAsync();

                // Optional comments
                if (authorId != null)
                {
                    var c1 = new Comment { MissionId = allMissions[0].Id, UserId = authorId, Content = "Proceed with caution." };
                    var c2 = new Comment { MissionId = allMissions[1].Id, UserId = authorId, Content = "Weather looks tough, prepare ropes." };
                    db.Comments.AddRange(c1, c2);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
