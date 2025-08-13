using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.Comment.ContentMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int MissionId { get; set; }
        public Mission Mission { get; set; } = null!;

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public bool IsDeleted { get; set; } = false;
    }
}