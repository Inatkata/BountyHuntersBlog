using System.Collections.Generic;
using System.Threading.Tasks;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface IMissionTagRepository : IRepository<MissionTag>
    {
        Task<List<MissionTag>> AllAsync();

        Task<MissionTag?> GetAsync(int missionId, int tagId);

        Task<bool> ExistsAsync(int missionId, int tagId);

        Task AddAsync(MissionTag entity);

        void Delete(MissionTag entity);

        Task SaveChangesAsync();

        // helpers
        Task<IReadOnlyList<int>> GetTagIdsForMissionAsync(int missionId);
        Task SetMissionTagsAsync(int missionId, IEnumerable<int> tagIds);
    }
}