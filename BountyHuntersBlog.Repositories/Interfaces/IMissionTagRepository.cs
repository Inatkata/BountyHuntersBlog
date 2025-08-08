using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface IMissionTagRepository : IRepository<MissionTag>
    {
        Task<IEnumerable<MissionTag>> GetByMissionIdAsync(int missionId);
        
        Task<IEnumerable<MissionTag>> GetByTagIdAsync(int tagId);
        
        Task<MissionTag?> GetByMissionAndTagIdAsync(int missionId, int tagId);
        
        Task<bool> ExistsAsync(int missionId, int tagId);
        
        Task RemoveByMissionAndTagIdAsync(int missionId, int tagId);
    }
}
