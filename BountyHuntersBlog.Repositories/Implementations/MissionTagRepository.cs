// Implementations/MissionTagRepository.cs
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class MissionTagRepository : Repository<MissionTag>, IMissionTagRepository
    {
        public MissionTagRepository(BountyHuntersDbContext ctx) : base(ctx) { }

        public Task<bool> LinkExistsAsync(int missionId, int tagId) =>
            _dbSet.AnyAsync(mt => mt.MissionId == missionId && mt.TagId == tagId);
    }
}