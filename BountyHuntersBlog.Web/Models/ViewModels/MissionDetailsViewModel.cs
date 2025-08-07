using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class MissionDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string FeaturedImageUrl { get; set; }

        public DateTime MissionDate { get; set; }

        public string? AuthorUsername { get; set; }

        public IEnumerable<MissionComment> Comments { get; set; }

        public int LikeCount { get; set; }

        public Guid CurrentHunterId { get; set; }
    }
}