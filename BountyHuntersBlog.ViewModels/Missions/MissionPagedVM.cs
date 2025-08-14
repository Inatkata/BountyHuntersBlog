using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionsPagedVM
    {
        public string? Q { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalCount { get; set; }

        public IEnumerable<MissionCardVM> Items { get; set; } = Enumerable.Empty<MissionCardVM>();
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = Enumerable.Empty<SelectListItem>();

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / Math.Max(1, PageSize));
    }

    public class MissionCardVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string ImageUrl { get; set; } = "/img/placeholder.png";
        public int Likes { get; set; }
        public int Comments { get; set; }
    }
}