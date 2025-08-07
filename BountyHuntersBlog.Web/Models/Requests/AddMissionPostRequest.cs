using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.Models.Requests
{
    public class AddMissionPostRequest
    {
        public string? AuthorId { get; set; }

        public string Title { get; set; }
        public string PageTitle { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public IFormFile FeaturedImage { get; set; }
        public string FeaturedImageUrl { get; set; }  // ако го използваш
        public DateTime MissionDate { get; set; }
        public string UrlHandle { get; set; }
        public MissionStatus Status { get; set; }
        public bool Visible { get; set; }
        public List<string> SelectedFactions { get; set; } = new();
        public List<SelectListItem> Factions { get; set; } = new();

    }
}
