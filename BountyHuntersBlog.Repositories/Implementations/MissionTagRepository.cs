using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class MissionTagRepository : Repository<MissionTag>, IMissionTagRepository
    {
        public MissionTagRepository(BountyHuntersDbContext context)
            : base(context) { }
        public async Task<IEnumerable<MissionTag>> GetByMissionIdAsync(int missionId)
            => await Context.MissionTags
                .Where(mt => mt.MissionId == missionId)
                .ToListAsync();
        public async Task<IEnumerable<MissionTag>> GetByTagIdAsync(int tagId)
            => await Context.MissionTags
                .Where(mt => mt.TagId == tagId)
                .ToListAsync();
        public async Task<MissionTag?> GetByMissionAndTagIdAsync(int missionId, int tagId)
            => await DbSet.FirstOrDefaultAsync(mt => mt.MissionId == missionId && mt.TagId == tagId);
        public async Task<bool> ExistsAsync(int missionId, int tagId)
            => await DbSet.AnyAsync(mt => mt.MissionId == missionId && mt.TagId == tagId);
        public async Task RemoveByMissionAndTagIdAsync(int missionId, int tagId)
        {
            var missionTag = await GetByMissionAndTagIdAsync(missionId, tagId);
            if (missionTag != null)
            {
                Delete(missionTag);
                await SaveChangesAsync();
            }
        }
}
