using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IMissionLikeRepository
    {
        Task AddLike(MissionLike like);
        Task<bool> AlreadyLiked(Guid missionPostId, string ApplicationUserId);
        Task<int> GetTotalLikesAsync(Guid missionPostId);
    }
}