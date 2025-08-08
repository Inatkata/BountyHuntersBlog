using BountyHuntersBlog.Data.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.CommentContentMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public int MissionId { get; set; }
        public Mission Mission { get; set; } = null!;

        [Required]
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}