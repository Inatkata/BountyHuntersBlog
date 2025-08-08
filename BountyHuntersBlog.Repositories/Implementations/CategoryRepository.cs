using BountyHuntersBlog.Data.Models;

using Microsoft.EntityFrameworkCore;
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Repositories.Base;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BountyHuntersDbContext context)
            : base(context)
        {
        }

        public async Task<Category?> GetByNameAsync(string name)
            => await DbSet.FirstOrDefaultAsync(c => c.Name == name);

        public async Task<IEnumerable<Category>> GetCategoriesByMissionIdAsync(int missionId)
            => await Context.Missions
                .Where(m => m.Id == missionId)
                .Select(m => m.Category)
                .ToListAsync();


        // **Променено string вместо int**
        public async Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(string userId)
            => await Context.Missions
                .Where(m => m.AuthorId == userId)
                .SelectMany(m => m.Category != null ? new[] { m.Category } : Array.Empty<Category>())
                .Distinct()
                .ToListAsync();

        public async Task<bool> ExistsAsync(int categoryId)
            => await DbSet.AnyAsync(c => c.Id == categoryId);
    }
}