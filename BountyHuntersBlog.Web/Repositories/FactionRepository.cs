using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Repositories;
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
        public async Task<IEnumerable<Faction>> GetAllAsync(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNumber,
            int pageSize)
        {
            var query = dbContext.Factions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(f => f.Name.Contains(searchQuery)
                                         || f.DisplayName.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = sortDirection == "desc"
                        ? query.OrderByDescending(f => f.Name)
                        : query.OrderBy(f => f.Name);
                }
                else if (sortBy.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = sortDirection == "desc"
                        ? query.OrderByDescending(f => f.DisplayName)
                        : query.OrderBy(f => f.DisplayName);
                }
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string? searchQuery)
        {
            var query = dbContext.Factions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(f => f.Name.Contains(searchQuery)
                                         || f.DisplayName.Contains(searchQuery));
            }

            return await query.CountAsync();
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
