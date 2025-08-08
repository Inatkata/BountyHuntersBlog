using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
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

        public async Task<IEnumerable<Faction>> GetSelectedFactionsAsync(List<Guid> selectedFactionIds)
        {
            return await dbContext.Factions
                .Where(x => selectedFactionIds.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<Faction?> GetByIdAsync(Guid id)
        {
            return await dbContext.Factions.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Faction> AddAsync(Faction faction)
        {
            await dbContext.Factions.AddAsync(faction);
            await dbContext.SaveChangesAsync();
            return faction;
        }

        public async Task<Faction?> UpdateAsync(Faction faction)
        {
            var existing = await dbContext.Factions.FirstOrDefaultAsync(f => f.Id == faction.Id);

            if (existing != null)
            {
                existing.Name = faction.Name;
                existing.Description = faction.Description;

                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }

        public async Task<Faction?> DeleteAsync(Guid id)
        {
            var existing = await dbContext.Factions.FirstOrDefaultAsync(f => f.Id == id);

            if (existing != null)
            {
                dbContext.Factions.Remove(existing);
                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }
    }
}
