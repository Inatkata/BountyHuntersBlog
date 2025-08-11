using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class MissionRepository : Repository<Mission>, IMissionRepository
{
    private readonly BountyHuntersDbContext _db;

    public MissionRepository(BountyHuntersDbContext context)
        : base(context)
    {
        _db = context;
    }

    public Task<Mission?> GetByIdWithIncludesAsync(int id)
        => _db.Set<Mission>()
            .Include(m => m.User)
            .Include(m => m.MissionTags).ThenInclude(mt => mt.Tag)
            .Include(m => m.Comments).ThenInclude(c => c.User)
            .Include(m => m.Comments).ThenInclude(c => c.Likes)
            .Include(m => m.Likes)
            .FirstOrDefaultAsync(m => m.Id == id);
}