using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class MissionDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string PageTitle { get; set; }

        public string Content { get; set; }

        public string FeaturedImageUrl { get; set; }
        public string ShortDescription { get; set; }
        public DateTime MissionDate { get; set; }

        public string UrlHandle { get; set; }

        public List<Faction> Factions { get; set; }
        public Hunter Author { get; set; }


        public int TotalLikes { get; set; }

        public bool Liked { get; set; }

        public List<MissionCommentViewModel> Comments { get; set; } = new();


        public bool Visible { get; set; }
        public string CommentDescription { get; set; }
    }
}
