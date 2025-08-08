using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        
        Task<Tag?> GetByNameAsync(string name);
       
        Task<IEnumerable<Tag>> GetTagsByMissionIdAsync(int missionId);
    }
}
