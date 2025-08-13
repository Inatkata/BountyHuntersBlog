using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Data.Models
{
    public class MissionTag
    {
        [Required]
        public int MissionId { get; set; }
        public Mission Mission { get; set; } = null!;

        [Required]
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}