using System;
using System.Collections.Generic;

namespace BountyHuntersBlog.Data.Models
{
    public class Mission
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool IsCompleted { get; set; }

        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<MissionTag> MissionTags { get; set; } = new List<MissionTag>();
    }
}