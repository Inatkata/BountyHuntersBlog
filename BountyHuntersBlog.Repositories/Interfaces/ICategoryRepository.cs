using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<IEnumerable<Category>> GetCategoriesByMissionIdAsync(int missionId);
    // смени типа тук!
    Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(string userId);
    Task<bool> ExistsAsync(int categoryId);
}