// Implementations/TagRepository.cs
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(BountyHuntersDbContext ctx) : base(ctx) { }

        public Task<Tag?> GetByNameAsync(string name) =>
            _dbSet.FirstOrDefaultAsync(t => t.Name == name);

        public Task<bool> ExistsByNameAsync(string name) =>
            _dbSet.AnyAsync(t => t.Name == name);
    }
}