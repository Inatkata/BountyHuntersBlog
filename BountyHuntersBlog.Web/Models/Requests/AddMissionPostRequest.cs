using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Requests
{
    public class AddMissionPostRequest
    {
        [Required]
        public string Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? Content { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public string? UrlHandle { get; set; }

        public DateTime MissionDate { get; set; }

        public bool Visible { get; set; }

        // Избрани фракции от формата
        public List<Guid> SelectedFactions { get; set; }

        // За dropdown списъка във View-то
        public IEnumerable<SelectListItem> FactionList { get; set; }
    }
}