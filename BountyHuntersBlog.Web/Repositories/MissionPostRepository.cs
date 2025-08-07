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

        public async Task<MissionPost> AddAsync(MissionPost post)
        {
            await dbContext.MissionPosts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<MissionPost> DeleteAsync(Guid id)
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
                .Include(p => p.Factions)
                .Include(p => p.Author)
                .ToListAsync();
        }

        public async Task<MissionPost> GetAsync(Guid id)
        {
            return await dbContext.MissionPosts
                .Include(p => p.Factions)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<MissionPost> UpdateAsync(MissionPost post)
        {
            var existing = await dbContext.MissionPosts
                .Include(p => p.Factions)
                .FirstOrDefaultAsync(p => p.Id == post.Id);

            if (existing != null)
            {
                existing.Title = post.Title;
                existing.Content = post.Content;
                existing.UrlHandle = post.UrlHandle;
                existing.FeaturedImageUrl = post.FeaturedImageUrl;
                existing.MissionDate = post.MissionDate;
                existing.Visible = post.Visible;
                existing.Factions = post.Factions;

                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }
    }
}
