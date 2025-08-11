// Interfaces/IMissionTagRepository.cs
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface IMissionTagRepository : IRepository<MissionTag>
    {
        Task<bool> LinkExistsAsync(int missionId, int tagId);
    }
}