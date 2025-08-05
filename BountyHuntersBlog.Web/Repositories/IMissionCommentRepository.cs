using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Web.Repositories
{
    public interface IMissionCommentRepository
    {
        Task<MissionComment> AddAsync(MissionComment comment);
        Task<IEnumerable<MissionComment>> GetAllAsync(Guid missionPostId);
    }
}