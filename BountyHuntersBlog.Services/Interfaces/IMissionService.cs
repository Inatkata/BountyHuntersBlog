using System.Collections.Generic;
using System.Threading.Tasks;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionService 
    {
        Task<IEnumerable<MissionTagDto>> GetAllAsync(int page, int pageSize);

        // composite key
        Task<MissionTagDto?> GetAsync(int missionId, int tagId);

        Task CreateAsync(MissionTagDto dto);

        // not meaningful for m2m; kept for symmetry
        Task UpdateAsync(int missionId, int tagId, MissionTagDto dto);

        Task DeleteAsync(int missionId, int tagId);

        Task<bool> ExistsAsync(int missionId, int tagId);

        // helpers for Mission Create/Edit screens
        Task<IReadOnlyList<int>> GetTagIdsForMissionAsync(int missionId);
        Task SetMissionTagsAsync(int missionId, IEnumerable<int> tagIds);
    }
}