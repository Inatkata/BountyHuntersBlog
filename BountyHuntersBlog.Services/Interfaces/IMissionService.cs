using System.Collections.Generic;
using System.Threading.Tasks;
using BountyHuntersBlog.Services.DTOs;

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
        Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(
            string? q, int? categoryId, int? tagId, int page, int pageSize);
    }
}