
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ITagService 
    {
        Task<IEnumerable<TagDto>> GetAllAsync(int page, int pageSize);
        Task<TagDto?> GetByIdAsync(int id);
        Task CreateAsync(TagDto dto);
        Task UpdateAsync(int id, TagDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
