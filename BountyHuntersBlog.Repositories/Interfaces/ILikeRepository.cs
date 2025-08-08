using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<IEnumerable<Like>> GetLikesByMissionIdAsync(int missionId);
        
        Task<IEnumerable<Like>> GetLikesByUserIdAsync(string userId);
        
        Task<bool> ExistsAsync(int missionId, string userId);
        
        Task RemoveByMissionAndUserIdAsync(int missionId, string userId);
        
        Task<int> CountLikesByMissionIdAsync(int missionId);
        
        Task<int> CountLikesByUserIdAsync(string userId);
    }
}
