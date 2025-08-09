using BountyHuntersBlog.Services.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionsIndexViewModel
    {
        public string? Q { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public IReadOnlyList<MissionDto> Items { get; set; } = Array.Empty<MissionDto>();
        public int TotalCount { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}