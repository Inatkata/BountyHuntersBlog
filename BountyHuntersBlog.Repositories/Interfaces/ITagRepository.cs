// Interfaces/ITagRepository.cs
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<Tag?> GetByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name);
    }
}