
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class MissionPostRepository : IMissionPostRepository
    {
        private readonly BountyHuntersDbContext dbContext;

        public MissionPostRepository(BountyHuntersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<MissionPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.MissionPosts
                .Include(m => m.Factions)
                .Include(m => m.Author) 
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<MissionPost>> GetAllAsync()
        {
            return await dbContext.MissionPosts
                .Include(m => m.Factions)
                .ToListAsync();
        }

        public async Task<MissionPost?> GetAsync(Guid id)
        {
            return await dbContext.MissionPosts
                .Include(m => m.Factions)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MissionPost> AddAsync(MissionPost mission, List<Guid> factionIds)
        {
            mission.Factions = new List<Faction>();
            foreach (var factionId in factionIds)
            {
                var faction = await dbContext.Factions.FindAsync(factionId);
                if (faction != null)
                {
                    mission.Factions.Add(faction);
                }
            }

            await dbContext.MissionPosts.AddAsync(mission);
            await dbContext.SaveChangesAsync();

            return mission;
        }
        public async Task<MissionPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.MissionPosts
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<MissionPost?> UpdateAsync(MissionPost mission, List<Guid> factionIds)
        {
            var existing = await dbContext.MissionPosts
                .Include(m => m.Factions)
                .FirstOrDefaultAsync(x => x.Id == mission.Id);

            if (existing != null)
            {
                existing.Title = mission.Title;
                existing.PageTitle = mission.PageTitle;
                existing.Content = mission.Content;
                existing.ShortDescription = mission.ShortDescription;
                existing.UrlHandle = mission.UrlHandle;
                existing.FeaturedImageUrl = mission.FeaturedImageUrl;
                existing.MissionDate = mission.MissionDate;
                existing.Status = mission.Status;
                existing.Visible = mission.Visible;

                // Update factions
                existing.Factions.Clear();
                foreach (var factionId in factionIds)
                {
                    var faction = await dbContext.Factions.FindAsync(factionId);
                    if (faction != null)
                    {
                        existing.Factions.Add(faction);
                    }
                }

                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }

        public async Task<MissionPost?> DeleteAsync(Guid id)
        {
            var mission = await dbContext.MissionPosts.FindAsync(id);
            if (mission != null)
            {
                dbContext.MissionPosts.Remove(mission);
                await dbContext.SaveChangesAsync();
                return mission;
            }

            return null;
        }
    }
}
