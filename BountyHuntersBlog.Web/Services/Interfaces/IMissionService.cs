using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionService
    {
        Task<IEnumerable<MissionPost>> GetAllAsync();
        Task<MissionPost?> GetByIdAsync(Guid id);
        Task AddAsync(AddMissionPostRequest request, Guid authorId);
        Task UpdateAsync(EditMissionPostRequest request);
        Task DeleteAsync(Guid id);
    }
}