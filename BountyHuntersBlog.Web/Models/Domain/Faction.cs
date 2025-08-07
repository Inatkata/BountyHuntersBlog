using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Domain
{
    public class Faction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<MissionPost> MissionPosts { get; set; }
    }
}