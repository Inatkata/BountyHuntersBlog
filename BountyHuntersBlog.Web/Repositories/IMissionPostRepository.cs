using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IMissionPostRepository
    {
        Task<IEnumerable<MissionPost>> GetAllAsync();
        Task<MissionPost?> GetAsync(Guid id);
        Task<MissionPost?> GetByUrlHandleAsync(string urlHandle);
        Task<MissionPost> AddAsync(MissionPost missionPost);
        Task<MissionPost?> UpdateAsync(MissionPost missionPost);
        Task<MissionPost?> DeleteAsync(Guid id);
    }
}