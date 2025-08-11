using System;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Data.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int? MissionId { get; set; }
        public Mission? Mission { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}