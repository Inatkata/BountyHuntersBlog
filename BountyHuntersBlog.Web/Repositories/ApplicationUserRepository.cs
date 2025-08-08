using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly BountyHuntersDbContext BountyHuntersDbContext;

        public ApplicationUserRepository(BountyHuntersDbContext BountyHuntersDbContext)
        {
            this.BountyHuntersDbContext = BountyHuntersDbContext;
        }


        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            var ApplicationUsers = await BountyHuntersDbContext.Users.ToListAsync(); 

            var superAdminApplicationUser = await BountyHuntersDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (superAdminApplicationUser is not null)
            {
                ApplicationUsers.Remove(superAdminApplicationUser);
            }

            return ApplicationUsers;
        }

    }
}