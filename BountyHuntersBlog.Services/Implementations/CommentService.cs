using System.Security.Claims;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _comments;
        private readonly IMissionRepository _missions;

        public CommentService(ICommentRepository comments, IMissionRepository missions)
        {
            _comments = comments;
            _missions = missions;
        }

        public async Task AddAsync(int missionId, string text, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Unauthenticated.");
            var mission = await _missions.GetByIdAsync(missionId) ?? throw new KeyNotFoundException("Mission not found.");

            var c = new Comment
            {
                MissionId = mission.Id,
                Text = text.Trim(),
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            await _comments.AddAsync(c);
            await _comments.SaveChangesAsync();
        }

        public async Task DeleteAsync(int commentId, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Unauthenticated.");
            var c = await _comments.GetByIdAsync(commentId) ?? throw new KeyNotFoundException("Comment not found.");

            if (c.UserId != userId && !(user.IsInRole("Admin")))
                throw new UnauthorizedAccessException("Not allowed.");

            _comments.Delete(c);
            await _comments.SaveChangesAsync();
        }
    }
}