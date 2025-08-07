using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionPost
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string UrlHandle { get; set; }

        public string FeaturedImageUrl { get; set; }

        public DateTime MissionDate { get; set; }

        public bool Visible { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public ICollection<Faction> Factions { get; set; }

        public ICollection<MissionLike> Likes { get; set; }

        public ICollection<MissionComment> Comments { get; set; }
    }
}