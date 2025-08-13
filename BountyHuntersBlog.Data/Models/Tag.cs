using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.Data.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.Tag.NameMaxLength)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }

        public ICollection<MissionTag> MissionTags { get; set; } = new List<MissionTag>();
    }
}