using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class AddMissionPostRequest
    {
        public string Title { get; set; }
        public string PageTitle { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }

        public IFormFile FeaturedImage { get; set; }

        public DateTime MissionDate { get; set; }

        public string UrlHandle { get; set; }

        public MissionStatus Status { get; set; }

        public bool Visible { get; set; }

        public List<Guid> SelectedFactions { get; set; }

        public List<SelectListItem> Factions { get; set; }
    }
}
