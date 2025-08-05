using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime MissionDate { get; set; }

        public string? AuthorId { get; set; }

        public Hunter? Author { get; set; }



        public bool Visible { get; set; }

        public ICollection<Faction> Factions { get; set; }
        public ICollection<MissionLike> Likes { get; set; }
        public ICollection<MissionComment> Comments { get; set; }

        public MissionStatus Status { get; set; }
    }
}