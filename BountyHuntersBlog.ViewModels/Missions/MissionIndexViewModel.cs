// ViewModels/Missions/MissionIndexViewModel.cs
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionIndexViewModel
    {
        public string? Q { get; set; }
        public string? Search { get => Q; set => Q = value; }   // back-compat

        public int? CategoryId { get; set; }    // да е nullable (Views ползват HasValue)
        public int? TagId { get; set; }         // да е nullable

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public List<MissionListItemViewModel> Items { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = new List<SelectListItem>();

        // back-compat за стари Views
        public BountyHuntersBlog.ViewModels.Shared.PaginationViewModel Pagination { get; set; }
            = new BountyHuntersBlog.ViewModels.Shared.PaginationViewModel();
    }
}