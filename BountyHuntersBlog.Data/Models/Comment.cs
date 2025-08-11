using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BountyHuntersBlog.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int MissionId { get; set; }
        public Mission Mission { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}