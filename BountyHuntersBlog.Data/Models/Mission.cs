
using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.Data.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.MissionTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ModelConstants.MissionDescriptionMaxLength)]
        public string Description { get; set; } = null!;
        [Required]
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
        [Required]
        public DateTime CreatedOn { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;


        // Връзки
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<MissionTag> MissionTags { get; set; } = new List<MissionTag>();
    }
}