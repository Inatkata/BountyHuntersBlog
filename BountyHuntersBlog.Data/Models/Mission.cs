using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.Data.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.Mission.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ModelConstants.Mission.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public bool IsCompleted { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<MissionTag> MissionTags { get; set; } = new List<MissionTag>();
    }
}