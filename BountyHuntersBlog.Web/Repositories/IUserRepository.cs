using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Repositories
{
    public interface IHunterRepository
    {
        Task<IEnumerable<IdentityHunter>> GetAll();
    }
}
