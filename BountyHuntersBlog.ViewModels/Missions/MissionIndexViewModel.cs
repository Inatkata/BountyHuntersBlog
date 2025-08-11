using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.ViewModels
{
    public class MissionsIndexViewModel
    {
        public string? Q { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = Array.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = Array.Empty<SelectListItem>();

        public IReadOnlyList<MissionListItemViewModel> Items { get; set; } = Array.Empty<MissionListItemViewModel>();
        public PaginationViewModel Pagination { get; set; } = new();
    }
}