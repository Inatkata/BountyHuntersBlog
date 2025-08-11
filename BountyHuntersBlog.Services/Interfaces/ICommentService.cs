using System.Security.Claims;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ICommentService
    {
        Task AddAsync(int missionId, string text, ClaimsPrincipal user);
        Task DeleteAsync(int commentId, ClaimsPrincipal user);
    }
}