using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByMissionIdAsync(int missionId);
        
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
        
        Task<Comment?> GetCommentByIdAsync(int commentId);
        
        Task<bool> ExistsAsync(int commentId);
        
        Task RemoveByIdAsync(int commentId);
        
        Task<int> CountCommentsByMissionIdAsync(int missionId);
        
        Task<int> CountCommentsByUserIdAsync(int userId);
    }
}
