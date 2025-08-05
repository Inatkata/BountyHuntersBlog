using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Web.Repositories
{
    public interface IMissionPostRepository
    {
        Task<List<MissionPost>> GetAllAsync();
        Task<MissionPost?> GetAsync(Guid id);
        Task<MissionPost> AddAsync(MissionPost mission, List<Guid> factionIds);
        Task<MissionPost?> UpdateAsync(MissionPost mission, List<Guid> factionIds);
        Task<MissionPost?> DeleteAsync(Guid id);
    }
}