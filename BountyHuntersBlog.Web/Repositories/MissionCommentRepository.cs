using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
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

        public async Task<IEnumerable<MissionComment>> GetAllByPostIdAsync(Guid missionPostId)
        {
            return await dbContext.MissionComments
                .Include(c => c.Hunter)
                .Where(c => c.MissionPostId == missionPostId)
                .OrderByDescending(c => c.DateAdded)
                .ToListAsync();
        }
    }
}