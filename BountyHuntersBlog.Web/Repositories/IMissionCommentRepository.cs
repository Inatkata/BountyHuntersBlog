using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IMissionCommentRepository
    {
        Task<MissionComment> AddAsync(MissionComment comment);
        Task<IEnumerable<MissionComment>> GetAllByPostIdAsync(Guid missionPostId);
    }
}