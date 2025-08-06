using BountyHuntersBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class HunterRepository : IHunterRepository
    {
        private readonly AuthDbContext authDbContext;

        public HunterRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }


        public async Task<IEnumerable<IdentityHunter>> GetAll()
        {
            var Hunters = await authDbContext.Hunters.ToListAsync();

            var superAdminHunter = await authDbContext.Hunters
                .FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (superAdminHunter is not null)
            {
                Hunters.Remove(superAdminHunter);
            }

            return Hunters;
        }
    }
}
