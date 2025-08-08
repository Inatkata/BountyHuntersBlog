using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<IEnumerable<Like>> GetLikesByMissionIdAsync(int missionId);
        
        Task<IEnumerable<Like>> GetLikesByUserIdAsync(int userId);
        
        Task<bool> ExistsAsync(int missionId, int userId);
        
        Task RemoveByMissionAndUserIdAsync(int missionId, int userId);
        
        Task<int> CountLikesByMissionIdAsync(int missionId);
        
        Task<int> CountLikesByUserIdAsync(int userId);
    }
}
