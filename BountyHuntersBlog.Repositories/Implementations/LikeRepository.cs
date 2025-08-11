// Implementations/LikeRepository.cs
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(BountyHuntersDbContext ctx) : base(ctx) { }

        public Task<Like?> FindForMissionAsync(string userId, int missionId) =>
            _dbSet.FirstOrDefaultAsync(l => l.UserId == userId && l.MissionId == missionId);

        public Task<Like?> FindForCommentAsync(string userId, int commentId) =>
            _dbSet.FirstOrDefaultAsync(l => l.UserId == userId && l.CommentId == commentId);
    }
}