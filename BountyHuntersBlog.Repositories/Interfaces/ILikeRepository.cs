using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<Like?> FindMissionLikeAsync(int missionId, string userId);
        Task<Like?> FindCommentLikeAsync(int commentId, string userId);
        Task AddAsync(Like like);
        Task RemoveAsync(Like like);
        Task<int> CountForMissionAsync(int missionId);
        Task<int> CountForCommentAsync(int commentId);
        Task SaveChangesAsync();
    }
}