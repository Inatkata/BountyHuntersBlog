using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BountyHuntersDbContext context)
            : base(context) { }
        public async Task<Category?> GetByNameAsync(string name)
            => await DbSet.FirstOrDefaultAsync(c => c.Name == name);
        public async Task<IEnumerable<Category>> GetCategoriesByMissionIdAsync(int missionId)
            => await Context.MissionCategories
                .Where(mc => mc.MissionId == missionId)
                .Select(mc => mc.Category)
                .ToListAsync();
        public async Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(int userId)
            => await Context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        public async Task<bool> ExistsAsync(int categoryId)
            => await DbSet.AnyAsync(c => c.Id == categoryId);

    }
}
