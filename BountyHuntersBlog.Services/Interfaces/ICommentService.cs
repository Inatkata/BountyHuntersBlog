using BountyHuntersBlog.Services.DTOs;
namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllAsync(int page, int pageSize);
        Task<CommentDto?> GetByIdAsync(int id);
        Task CreateAsync(CommentDto dto);
        Task UpdateAsync(int id, CommentDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<CommentDto>> GetCommentsByMissionIdAsync(int missionId);
        Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(string userId);
    }
}