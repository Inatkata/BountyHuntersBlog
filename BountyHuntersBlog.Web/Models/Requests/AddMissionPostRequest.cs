using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class AddMissionPostRequest
    {
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string UrlHandle { get; set; }

        public string FeaturedImageUrl { get; set; }
        public string ShortDescription { get; set; }

        public DateTime MissionDate { get; set; }

        public bool Visible { get; set; }

        public string AuthorId { get; set; }

        public List<Guid> SelectedFactions { get; set; } = new();
    }
}