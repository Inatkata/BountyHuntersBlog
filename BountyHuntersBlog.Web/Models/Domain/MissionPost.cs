
using System.ComponentModel.DataAnnotations;
using static BountyHuntersBlog.Constants.EntityConstants.MissionPost;
namespace BountyHuntersBlog.Models.Domain
{
    public class MissionPost
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;
        [Required]
        [MaxLength(PageTitleMaxLength)]
        public string PageTitle { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string ShortDescription { get; set; } = null!;
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;
        [Required]
        public string FeaturedImageUrl { get; set; } = null!;
        [Required]
        [MaxLength(UrlHandleMaxLength)]
        public string UrlHandle { get; set; } = null!;
        public DateTime MissionDate { get; set; }
        public bool Visible { get; set; }
        
        public MissionStatus Status { get; set; }
        public string PostedByUserId { get; set; }
        public ApplicationUser PostedByUser { get; set; }

        public string? TakenByApplicationUserId { get; set; }
        public ApplicationUser? TakenByApplicationUser { get; set; }



        public ICollection<MissionLike> Likes { get; set; } = new List<MissionLike>();
        public ICollection<MissionComment> Comments { get; set; } = new List<MissionComment>();
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();
    }

}