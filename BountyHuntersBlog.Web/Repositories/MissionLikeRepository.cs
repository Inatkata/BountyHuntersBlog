using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;

using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class MissionLikeRepository : IMissionLikeRepository
    {
        private readonly BountyHuntersDbContext dbContext;

        public MissionLikeRepository(BountyHuntersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddLike(MissionLike like)
        {
            await dbContext.MissionLikes.AddAsync(like);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> AlreadyLiked(Guid missionPostId, string ApplicationUserId)
        {
            return await dbContext.MissionLikes
                .AnyAsync(x => x.MissionPostId == missionPostId && x.ApplicationUserId.ToString() == ApplicationUserId);
        }

        public async Task<int> GetTotalLikesAsync(Guid missionPostId)
        {
            return await dbContext.MissionLikes
                .CountAsync(x => x.MissionPostId == missionPostId);
        }

        public async Task<List<MissionLike>> GetLikesByMissionIdAsync(Guid missionPostId)
        {
            return await dbContext.MissionLikes
                .Where(x => x.MissionPostId == missionPostId)
                .ToListAsync();
        }


    }
}