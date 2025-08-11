// Interfaces/ICategoryRepository.cs
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name);
    }
}