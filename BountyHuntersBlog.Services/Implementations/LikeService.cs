using System.Security.Claims;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likes;
        private readonly IMissionRepository _missions;
        private readonly ICommentRepository _comments;

        public LikeService(ILikeRepository likes, IMissionRepository missions, ICommentRepository comments)
        {
            _likes = likes;
            _missions = missions;
            _comments = comments;
        }

        public async Task LikeMissionAsync(int missionId, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Unauthenticated.");
            var mission = await _missions.GetByIdWithIncludesAsync(missionId) ?? throw new KeyNotFoundException("Mission not found.");

            if (!mission.Likes.Any(l => l.UserId == userId))
            {
                await _likes.AddAsync(new Like { MissionId = mission.Id, UserId = userId, CreatedOn = DateTime.UtcNow });
                await _likes.SaveChangesAsync();
            }
        }

        public async Task<int> LikeCommentAsync(int commentId, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Unauthenticated.");
            var comment = await _comments.GetByIdWithIncludesAsync(commentId) ?? throw new KeyNotFoundException("Comment not found.");

            if (!comment.Likes.Any(l => l.UserId == userId))
            {
                await _likes.AddAsync(new Like { CommentId = comment.Id, UserId = userId, CreatedOn = DateTime.UtcNow });
                await _likes.SaveChangesAsync();
            }

            return comment.MissionId;
        }
    }
}
