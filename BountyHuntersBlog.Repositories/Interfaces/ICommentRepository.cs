using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByMissionIdAsync(int missionId);

        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string authorId);


        Task<Comment?> GetCommentByIdAsync(int commentId);
        
        Task<bool> ExistsAsync(int commentId);
        
        Task RemoveByIdAsync(int commentId);
        
        Task<int> CountCommentsByMissionIdAsync(int missionId);
        
        Task<int> CountCommentsByUserIdAsync(string userId);
    }
}
