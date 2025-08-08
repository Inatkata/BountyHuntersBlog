using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class MissionPostRepository : IMissionPostRepository
    {
        private readonly BountyHuntersDbContext dbContext;

        public MissionPostRepository(BountyHuntersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<MissionPost> AddAsync(MissionPost missionPost)
        {
            await dbContext.MissionPosts.AddAsync(missionPost);
            await dbContext.SaveChangesAsync();
            return missionPost;
        }

        public async Task<MissionPost?> DeleteAsync(Guid id)
        {
            var existing = await dbContext.MissionPosts.FindAsync(id);

            if (existing != null)
            {
                dbContext.MissionPosts.Remove(existing);
                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }
        public async Task<IEnumerable<MissionPost>> GetAllAsync()
        {
            return await dbContext.MissionPosts
                .Where(x => x.AuthorId != null) // <– филтър само за валидни автори
                .Include(x => x.Author)
                .Include(x => x.Factions)
                .Include(x => x.MissionLikes)
                .Include(x => x.MissionComments)
                .ToListAsync();
        }






        public async Task<MissionPost?> GetAsync(Guid id)
        {
            return await dbContext.MissionPosts
                .Include(x => x.Author)
                .Include(x => x.Factions)
                .Include(x => x.MissionLikes)
                .Include(x => x.MissionComments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MissionPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.MissionPosts
                .Include(x => x.Author)
                .Include(x => x.Factions)
                .Include(x => x.MissionLikes)
                .Include(x => x.MissionComments)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<MissionPost?> UpdateAsync(MissionPost missionPost)
        {
            var existing = await dbContext.MissionPosts
                .Include(x => x.Factions)
                .FirstOrDefaultAsync(x => x.Id == missionPost.Id);

            if (existing != null)
            {
                existing.Title = missionPost.Title;
                existing.Content = missionPost.Content;
                existing.ShortDescription = missionPost.ShortDescription;
                existing.FeaturedImageUrl = missionPost.FeaturedImageUrl;
                existing.UrlHandle = missionPost.UrlHandle;
                existing.MissionDate = missionPost.MissionDate;
                existing.Visible = missionPost.Visible;
                existing.Factions = missionPost.Factions;
                existing.AuthorId = missionPost.AuthorId;

                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }
    }
}
