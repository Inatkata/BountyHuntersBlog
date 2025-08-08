using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(BountyHuntersDbContext context)
            : base(context) { }
        public async Task<IEnumerable<Like>> GetLikesByMissionIdAsync(int missionId)
            => await Context.Likes
                .Where(l => l.MissionId == missionId)
                .ToListAsync();
        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(int userId)
            => await Context.Likes
                .Where(l => l.UserId == userId)
                .ToListAsync();
        public async Task<bool> ExistsAsync(int missionId, int userId)
            => await Context.Likes.AnyAsync(l => l.MissionId == missionId && l.UserId == userId);
        public async Task RemoveByMissionAndUserIdAsync(int missionId, int userId)
        {
            var like = await Context.Likes
                .FirstOrDefaultAsync(l => l.MissionId == missionId && l.UserId == userId);
            if (like != null)
            {
                Delete(like);
                await SaveChangesAsync();
            }
        }
        public async Task<int> CountLikesByMissionIdAsync(int missionId)
            => await Context.Likes.CountAsync(l => l.MissionId == missionId);
        public async Task<int> CountLikesByUserIdAsync(int userId)
            => await Context.Likes.CountAsync(l => l.UserId == userId);
    }
}
