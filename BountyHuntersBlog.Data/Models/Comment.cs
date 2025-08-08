using System;
using System.Collections.Generic;

namespace BountyHuntersBlog.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }

        public int MissionId { get; set; }
        public Mission Mission { get; set; } = null!;

        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}