using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BountyHuntersDbContext>();

        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category { Name = "Bounty Missions" },
                new Category { Name = "Exploration" },
                new Category { Name = "Rescue" }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Tags.Any())
        {
            db.Tags.AddRange(
                new Tag { Name = "Dangerous" },
                new Tag { Name = "Urgent" },
                new Tag { Name = "Long-term" }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Missions.Any())
        {
            var firstCategory = db.Categories.First();
            var firstTag = db.Tags.First();

            db.Missions.Add(new Mission
            {
                Title = "Capture the Outlaw",
                Description = "Locate and capture the wanted outlaw in sector 9.",
                CategoryId = firstCategory.Id,
                AuthorId = db.Users.First().Id, // или админ ID
                MissionTags = new List<MissionTag>
                {
                    new MissionTag { TagId = firstTag.Id }
                }
            });

            await db.SaveChangesAsync();
        }
    }
}