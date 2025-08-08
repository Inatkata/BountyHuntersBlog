using BountyHuntersBlog.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionService
    {
        Task<IEnumerable<MissionDto>> GetAllAsync(int page, int pageSize);
        Task<MissionDto?> GetByIdAsync(int id);
        Task CreateAsync(MissionDto dto);
        Task UpdateAsync(int id, MissionDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}