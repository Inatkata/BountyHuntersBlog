using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IMissionPostRepository
    {
        Task<MissionPost?> GetByUrlHandleAsync(string urlHandle);
        Task<MissionPost?> GetByIdAsync(Guid id);

        Task<List<MissionPost>> GetAllAsync();
        Task<MissionPost?> GetAsync(Guid id);
        Task<MissionPost> AddAsync(MissionPost mission, List<Guid> factionIds);
        Task<MissionPost?> UpdateAsync(MissionPost mission, List<Guid> factionIds);
        Task<MissionPost?> DeleteAsync(Guid id);
    }
}