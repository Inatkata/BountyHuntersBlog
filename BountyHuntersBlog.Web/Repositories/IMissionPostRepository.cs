using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IMissionPostRepository
    {
        Task<IEnumerable<MissionPost>> GetAllAsync();
        Task<MissionPost> GetAsync(Guid id);
        Task<MissionPost> AddAsync(MissionPost post, List<Guid> selectedFactions);
        Task<MissionPost> UpdateAsync(MissionPost post, List<Guid> selectedFactions);

        Task<MissionPost> DeleteAsync(Guid id);
    }
}