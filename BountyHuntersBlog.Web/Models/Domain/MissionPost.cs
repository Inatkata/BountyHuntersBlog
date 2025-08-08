using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionPost
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? Content { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public string? UrlHandle { get; set; }

        public DateTime MissionDate { get; set; }

        public bool Visible { get; set; }
        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public Hunter Author { get; set; }

        public ICollection<MissionLike> MissionLikes { get; set; }
        public ICollection<MissionComment> MissionComments { get; set; }

        // Много към много с Factions
        public ICollection<Faction> Factions { get; set; }
    }
}