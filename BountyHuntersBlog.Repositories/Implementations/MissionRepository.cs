// Implementations/MissionRepository.cs
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class MissionRepository : Repository<Mission>, IMissionRepository
    {
        public MissionRepository(BountyHuntersDbContext ctx) : base(ctx) { }

        public Task<Mission?> GetByIdWithIncludesAsync(int id) =>
            _dbSet
                .Include(m => m.User)
                .Include(m => m.Category)
                .Include(m => m.MissionTags).ThenInclude(mt => mt.Tag)
                .Include(m => m.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

        public IQueryable<Mission> SearchQueryable(string? q, int? categoryId, int? tagId)
        {
            var query = _dbSet
                .Include(m => m.User)
                .Include(m => m.Category)
                .Include(m => m.MissionTags).ThenInclude(mt => mt.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var qq = q.Trim().ToLower();
                query = query.Where(m => m.Title.ToLower().Contains(qq) ||
                                         m.Description.ToLower().Contains(qq));
            }

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (tagId.HasValue)
                query = query.Where(m => m.MissionTags.Any(mt => mt.TagId == tagId.Value));

            return query;
        }
    }
}