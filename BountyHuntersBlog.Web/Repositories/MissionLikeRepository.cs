
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Web.Repositories
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

        public async Task<bool> AlreadyLiked(Guid missionPostId, string HunterId)
        {
            return await dbContext.MissionLikes.AnyAsync(x =>
                x.MissionPostId == missionPostId && x.HunterId == HunterId);
        }

        public async Task<int> GetTotalLikes(Guid missionPostId)
        {
            return await dbContext.MissionLikes.CountAsync(x => x.MissionPostId == missionPostId);
        }
    }
}