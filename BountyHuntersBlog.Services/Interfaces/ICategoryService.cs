using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CommentDto>> GetAllAsync(int page, int pageSize);
        Task<CommentDto?> GetByIdAsync(int id);
        Task CreateAsync(CommentDto dto);
        Task UpdateAsync(int id, CommentDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}