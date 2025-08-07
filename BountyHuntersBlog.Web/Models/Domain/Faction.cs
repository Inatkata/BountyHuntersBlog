using System.ComponentModel.DataAnnotations;
using static BountyHuntersBlog.Constants.EntityConstants.Faction;

namespace BountyHuntersBlog.Models.Domain
{
    public class Faction
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DisplayNameMaxLength)]
        public string DisplayName { get; set; } = null!;

        public ICollection<MissionPost> MissionPosts { get; set; } = new List<MissionPost>();
    }

}