// Interfaces/ILikeRepository.cs
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<Like?> FindForMissionAsync(string userId, int missionId);
        Task<Like?> FindForCommentAsync(string userId, int commentId);
    }
}