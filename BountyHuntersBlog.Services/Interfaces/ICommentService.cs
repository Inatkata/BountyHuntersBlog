// Services/Interfaces/ICommentService.cs
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> AddAsync(int missionId, string userId, string content);
        Task<bool> EditAsync(int id, string content);
        Task<bool> DeleteAsync(int id);
        Task<IReadOnlyList<CommentDto>> GetForMissionAsync(int missionId);
        Task<CommentDto?> GetByIdAsync(int id);
        Task<CommentDto> SoftDeleteAsync(int id);
        Task<IReadOnlyList<CommentDto>> AllAsync();
    }
}