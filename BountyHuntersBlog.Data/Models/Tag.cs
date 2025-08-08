using BountyHuntersBlog.Data.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Data.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.TagNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<MissionTag> MissionTags { get; set; } = new List<MissionTag>();
    }
}