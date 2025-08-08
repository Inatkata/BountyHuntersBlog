using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
        
        Task<IEnumerable<Category>> GetCategoriesByMissionIdAsync(int missionId);
        
        Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(int userId);
        
        Task<bool> ExistsAsync(int categoryId);
    }
}
