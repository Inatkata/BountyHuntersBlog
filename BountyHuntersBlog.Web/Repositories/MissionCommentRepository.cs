
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Web.Repositories
{
    public class MissionCommentRepository : IMissionCommentRepository
    {
        private readonly BountyHuntersDbContext dbContext;

        public MissionCommentRepository(BountyHuntersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<MissionComment> AddAsync(MissionComment comment)
        {
            await dbContext.MissionComments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<IEnumerable<MissionComment>> GetAllAsync(Guid missionPostId)
        {
            return await dbContext.MissionComments
                .Where(x => x.MissionPostId == missionPostId)
                .ToListAsync();
        }
    }
}