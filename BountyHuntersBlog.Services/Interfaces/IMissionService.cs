using System.Security.Claims;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionService
    {
        Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(string? q, int? categoryId, int? tagId, int page, int pageSize);
        Task<MissionWithStatsDto?> GetByIdWithStatsAsync(int id, ClaimsPrincipal? user = null);
        Task<MissionDto?> GetByIdAsync(int id);
        Task CreateAsync(MissionDto dto, ClaimsPrincipal user);
        Task UpdateAsync(int id, MissionDto dto, ClaimsPrincipal user);
        Task DeleteAsync(int id);
    }
}