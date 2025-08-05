using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Web.Repositories
{
    public interface IMissionLikeRepository
    {
        Task<int> GetTotalLikes(Guid missionPostId);
        Task AddLike(MissionLike like);
        Task<bool> AlreadyLiked(Guid missionPostId, string userId);
    }
}