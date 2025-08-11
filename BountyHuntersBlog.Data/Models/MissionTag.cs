// BountyHuntersBlog.Data/Models/MissionTag.cs
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Data.Models
{
    public class MissionTag
    {
        // Composite PK is configured via Fluent API
        [Required]
        public int MissionId { get; set; }
        public Mission Mission { get; set; } = null!;

        [Required]
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}