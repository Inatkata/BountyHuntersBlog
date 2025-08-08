using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class HunterRepository : IHunterRepository
    {
        private readonly BountyHuntersDbContext dbContext;

        public HunterRepository(BountyHuntersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Hunter> AddAsync(Hunter hunter)
        {
            await dbContext.Hunters.AddAsync(hunter);
            await dbContext.SaveChangesAsync();
            return hunter;
        }

        public async Task<Hunter?> DeleteAsync(Guid id)
        {
            var hunter = await dbContext.Hunters.FindAsync(id);
            if (hunter != null)
            {
                dbContext.Hunters.Remove(hunter);
                await dbContext.SaveChangesAsync();
                return hunter;
            }
            return null;
        }

        public async Task<IEnumerable<Hunter>> GetAllAsync()
        {
            return await dbContext.Hunters.ToListAsync();
        }

        public async Task<Hunter?> GetAsync(Guid id)
        {
            return await dbContext.Hunters.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Hunter?> UpdateAsync(Hunter hunter)
        {
            var existing = await dbContext.Hunters.FindAsync(hunter.Id);

            if (existing != null)
            {
                existing.DisplayName = hunter.DisplayName;
                existing.Bio = hunter.Bio;
                existing.ExperienceLevel = hunter.ExperienceLevel;
                existing.JoinedOn = hunter.JoinedOn;

                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }
    }
}