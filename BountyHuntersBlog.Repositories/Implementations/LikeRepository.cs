using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly BountyHuntersDbContext _db;

        public LikeRepository(BountyHuntersDbContext context)
            : base(context) { }

        public Task<Like?> FindMissionLikeAsync(int missionId, string userId) =>
            _db.Likes.FirstOrDefaultAsync(l => l.MissionId == missionId && l.UserId == userId);

        public Task<Like?> FindCommentLikeAsync(int commentId, string userId) =>
            _db.Likes.FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);

        public async Task AddAsync(Like like)
        {
            await _db.Likes.AddAsync(like);
        }

        public Task RemoveAsync(Like like)
        {
            _db.Likes.Remove(like);
            return Task.CompletedTask;
        }

        public Task<int> CountForMissionAsync(int missionId) =>
            _db.Likes.CountAsync(l => l.MissionId == missionId);

        public Task<int> CountForCommentAsync(int commentId) =>
            _db.Likes.CountAsync(l => l.CommentId == commentId);

        public Task SaveChangesAsync() => _db.SaveChangesAsync();
    }
}