using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class HunterRepository : IHunterRepository
    {
        private readonly BountyHuntersDbContext bountyHuntersDbContext;

        public HunterRepository(BountyHuntersDbContext bountyHuntersDbContext)
        {
            this.bountyHuntersDbContext = bountyHuntersDbContext;
        }


        public async Task<IEnumerable<Hunter>> GetAll()
        {
            var hunters = await bountyHuntersDbContext.Users.ToListAsync(); // .Users, не .Hunters

            var superAdminHunter = await bountyHuntersDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (superAdminHunter is not null)
            {
                hunters.Remove(superAdminHunter);
            }

            return hunters;
        }

    }
}