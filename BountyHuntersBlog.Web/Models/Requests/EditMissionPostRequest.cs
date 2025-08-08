using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Requests
{
    public class EditMissionPostRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? Content { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public string? UrlHandle { get; set; }

        public DateTime MissionDate { get; set; }

        public bool Visible { get; set; }

        public List<Guid> SelectedFactions { get; set; }

        public IEnumerable<SelectListItem> FactionList { get; set; }
    }
}