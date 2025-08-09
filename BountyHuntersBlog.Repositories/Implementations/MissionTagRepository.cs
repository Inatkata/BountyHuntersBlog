using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class MissionTagRepository : Repository<MissionTag>, IMissionTagRepository
    {
        private readonly BountyHuntersDbContext _db;
        public MissionTagRepository(BountyHuntersDbContext context)
            : base(context) { }

        public Task<List<MissionTag>> AllAsync() =>
            _db.MissionTags.AsNoTracking().ToListAsync();

        public Task<MissionTag?> GetAsync(int missionId, int tagId) =>
            _db.MissionTags.FirstOrDefaultAsync(x => x.MissionId == missionId && x.TagId == tagId);

        public Task<bool> ExistsAsync(int missionId, int tagId) =>
            _db.MissionTags.AnyAsync(x => x.MissionId == missionId && x.TagId == tagId);

        public async Task AddAsync(MissionTag entity)
        {
            await _db.MissionTags.AddAsync(entity);
        }

        public void Delete(MissionTag entity) => _db.MissionTags.Remove(entity);

        public Task SaveChangesAsync() => _db.SaveChangesAsync();

        public async Task<IReadOnlyList<int>> GetTagIdsForMissionAsync(int missionId)
        {
            var ids = await _db.MissionTags
                .Where(x => x.MissionId == missionId)
                .Select(x => x.TagId)
                .ToListAsync();

            return ids;
        }

        public async Task SetMissionTagsAsync(int missionId, IEnumerable<int> tagIds)
        {
            var desired = (tagIds ?? Enumerable.Empty<int>()).Distinct().ToHashSet();

            var existing = await _db.MissionTags
                .Where(x => x.MissionId == missionId)
                .ToListAsync();

            var existingSet = existing.Select(e => e.TagId).ToHashSet();

            var toAdd = desired.Except(existingSet)
                .Select(tid => new MissionTag { MissionId = missionId, TagId = tid })
                .ToList();

            var toRemove = existing.Where(e => !desired.Contains(e.TagId)).ToList();

            if (toAdd.Count > 0) await _db.MissionTags.AddRangeAsync(toAdd);
            if (toRemove.Count > 0) _db.MissionTags.RemoveRange(toRemove);
        }
    }
}
