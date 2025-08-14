// BountyHuntersBlog.ViewModels/Missions/MissionIndexViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionIndexViewModel
    {
        public string? Q { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public IEnumerable<MissionListItemVm> Items { get; set; } = new List<MissionListItemVm>();

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = new List<SelectListItem>();
    }
}