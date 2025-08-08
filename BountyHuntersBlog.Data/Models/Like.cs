using System;

namespace BountyHuntersBlog.Data.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int? MissionId { get; set; }
        public Mission? Mission { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}