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

        public async Task<int> GetTotalLikes(Guid missionPostId)
        {
            return await dbContext.MissionLikes
                .CountAsync(x => x.MissionPostId == missionPostId);
        }

        public async Task<bool> HasUserLiked(Guid missionPostId, Guid hunterId)
        {
            return await dbContext.MissionLikes.AnyAsync(x =>
                x.MissionPostId == missionPostId &&
                x.HunterId == hunterId);
        }

        public async Task<MissionLike?> AddLikeAsync(Guid missionPostId, Guid hunterId)
        {
            if (await HasUserLiked(missionPostId, hunterId))
                return null;

            var like = new MissionLike
            {
                Id = Guid.NewGuid(),
                MissionPostId = missionPostId,
                HunterId = hunterId
            };

            await dbContext.MissionLikes.AddAsync(like);
            await dbContext.SaveChangesAsync();

            return like;
        }
    }
}