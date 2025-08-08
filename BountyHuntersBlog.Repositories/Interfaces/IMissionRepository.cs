using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface IMissionRepository : IRepository<Mission>
    {
        Task<IEnumerable<Mission>> GetByAuthorAsync(string authorId);
        Task<bool> ExistsAsync(int id);
    }
}