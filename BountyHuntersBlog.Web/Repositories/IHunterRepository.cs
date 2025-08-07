using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Repositories
{
    public interface IHunterRepository
    {
        Task<IEnumerable<Hunter>> GetAll();

    }
}
