// Implementations/CategoryRepository.cs
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BountyHuntersDbContext ctx) : base(ctx) { }

        public Task<Category?> GetByNameAsync(string name) =>
            _dbSet.FirstOrDefaultAsync(c => c.Name == name);

        public Task<bool> ExistsByNameAsync(string name) =>
            _dbSet.AnyAsync(c => c.Name == name);
    }
}