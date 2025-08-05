using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
