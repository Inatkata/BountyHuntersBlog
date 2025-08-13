using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionService
    {
        Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(string? q, int? categoryId, int? tagId, int page, int pageSize);
        Task<MissionDetailsDto?> GetDetailsAsync(int id);

        Task ToggleMissionLikeAsync(int missionId, ClaimsPrincipal user);
        Task AddCommentAsync(int missionId, string content, ClaimsPrincipal user);
        Task ToggleCommentLikeAsync(int commentId, ClaimsPrincipal user);
        Task DeleteCommentAsync(int commentId, ClaimsPrincipal user);
        Task CreateAsync(MissionEditDto dto);

        Task<MissionEditDto?> GetEditAsync(int id);
        Task EditAsync(MissionEditDto dto);

        Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync();
        Task<IEnumerable<SelectListItem>> GetTagsSelectListAsync();
    }
}