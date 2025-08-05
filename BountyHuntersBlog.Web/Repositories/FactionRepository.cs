using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class FactionRepository : IFactionRepository
    {
        private readonly BountyHuntersDbContext dbContext;

        public FactionRepository(BountyHuntersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Faction>> GetAllAsync()
        {
            return await dbContext.Factions.ToListAsync();
        }

        public async Task<Faction?> GetAsync(Guid id)
        {
            return await dbContext.Factions.FindAsync(id);
        }

        public async Task<Faction> AddAsync(Faction faction)
        {
            await dbContext.Factions.AddAsync(faction);
            await dbContext.SaveChangesAsync();
            return faction;
        }

        public async Task<Faction?> UpdateAsync(Faction faction)
        {
            var existing = await dbContext.Factions.FindAsync(faction.Id);
            if (existing != null)
            {
                existing.Name = faction.Name;
                existing.DisplayName = faction.DisplayName;

                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }

        public async Task<Faction?> DeleteAsync(Guid id)
        {
            var faction = await dbContext.Factions.FindAsync(id);
            if (faction != null)
            {
                dbContext.Factions.Remove(faction);
                await dbContext.SaveChangesAsync();
                return faction;
            }

            return null;
        }
    }

}
