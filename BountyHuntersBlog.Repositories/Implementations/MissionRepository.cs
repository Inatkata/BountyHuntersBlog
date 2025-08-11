using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class MissionRepository : Repository<Mission>, IMissionRepository
{
    public MissionRepository(BountyHuntersDbContext ctx) : base(ctx) { }

    public Task<Mission?> GetByIdWithIncludesAsync(int id)
        => _db.Set<Mission>()
            .Include(m => m.User)
            .Include(m => m.MissionTags).ThenInclude(mt => mt.Tag)
            .Include(m => m.Comments).ThenInclude(c => c.User)
            .Include(m => m.Comments).ThenInclude(c => c.Likes)
            .Include(m => m.Likes)
            .FirstOrDefaultAsync(m => m.Id == id);
}