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

        public async Task<MissionPost> AddAsync(MissionPost post, List<Guid> selectedFactions)
        {
            var factions = await dbContext.Factions
                .Where(f => selectedFactions.Contains(f.Id))
                .ToListAsync();

            post.Factions = factions;

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
                .Include(p => p.Comments) 
                .ThenInclude(c => c.Hunter)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task<MissionPost> UpdateAsync(MissionPost post, List<Guid> selectedFactions)
        {
            var existing = await dbContext.MissionPosts
                .Include(p => p.Factions)
                .FirstOrDefaultAsync(p => p.Id == post.Id);

            if (existing != null)
            {
                existing.Title = post.Title;
                existing.ShortDescription = post.ShortDescription;
                existing.Content = post.Content;
                existing.FeaturedImageUrl = post.FeaturedImageUrl;
                existing.UrlHandle = post.UrlHandle;
                existing.MissionDate = post.MissionDate;
                existing.Visible = post.Visible;

                // Update factions
                var factions = await dbContext.Factions
                    .Where(f => selectedFactions.Contains(f.Id))
                    .ToListAsync();

                existing.Factions = factions;

                await dbContext.SaveChangesAsync();
                return existing;
            }

            return null;
        }

    }
}
