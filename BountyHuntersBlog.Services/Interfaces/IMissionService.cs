// Services/Interfaces/IMissionService.cs
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionService
    {
        Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(string? q, int? categoryId, int? tagId, int page, int pageSize);
        Task<MissionDto?> GetByIdAsync(int id);
        Task<MissionWithStatsDto?> GetDetailsAsync(int id);
        Task<int> CreateAsync(MissionDto dto);
        Task<bool> UpdateAsync(MissionDto dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}