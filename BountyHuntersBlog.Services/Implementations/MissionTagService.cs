// Services/Implementations/MissionTagService.cs
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Services.Implementations
{
    public class MissionTagService : IMissionTagService
    {
        private readonly IMissionTagRepository _missionTags;

        public MissionTagService(IMissionTagRepository missionTags)
        {
            _missionTags = missionTags;
        }

        public async Task UpdateTagsAsync(int missionId, IEnumerable<int> tagIds)
        {
            var desired = tagIds?.Distinct().ToHashSet() ?? new HashSet<int>();
            var existing = await _missionTags.AllAsQueryable()
                .Where(x => x.MissionId == missionId)
                .ToListAsync();

            foreach (var mt in existing.Where(x => !desired.Contains(x.TagId)))
                _missionTags.Delete(mt);

            foreach (var t in desired.Where(t => !existing.Any(x => x.TagId == t)))
                await _missionTags.AddAsync(new MissionTag { MissionId = missionId, TagId = t });

            await _missionTags.SaveChangesAsync();
        }
    }
}