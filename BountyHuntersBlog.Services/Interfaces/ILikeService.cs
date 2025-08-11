using System.Security.Claims;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ILikeService
    {
        Task LikeMissionAsync(int missionId, ClaimsPrincipal user);
        Task<int> LikeCommentAsync(int commentId, ClaimsPrincipal user); // returns missionId
    }
}