using BountyHuntersBlog.Services.DTOs;
namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionTagService : IRepositoryService<MissionTagDto>
    {
        Task<IEnumerable<MissionTagDto>> GetAllAsync(int page, int pageSize);
        Task<MissionTagDto?> GetByIdAsync(int id);
        Task CreateAsync(MissionTagDto dto);
        Task UpdateAsync(int id, MissionTagDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
       
    }
}
