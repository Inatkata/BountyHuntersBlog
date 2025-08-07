using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IMissionLikeRepository
    {
        Task<int> GetTotalLikes(Guid missionPostId);
        Task<MissionLike?> AddLikeAsync(Guid missionPostId, Guid hunterId);
        Task<bool> HasUserLiked(Guid missionPostId, Guid hunterId);
    }
}