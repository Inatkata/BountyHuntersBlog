using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAll();

    }
}
