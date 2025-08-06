

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string PageTitle { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string FeaturedImageUrl { get; set; } = null!;
        public string UrlHandle { get; set; } = null!;
        public DateTime MissionDate { get; set; }
        public bool Visible { get; set; }
        public string AuthorId { get; set; } = null!;
        public MissionStatus Status { get; set; }

        
        public Hunter Author { get; set; } = null!;
        public ICollection<MissionLike> Likes { get; set; } = new List<MissionLike>();
        public ICollection<MissionComment> Comments { get; set; } = new List<MissionComment>();
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();
    }

}