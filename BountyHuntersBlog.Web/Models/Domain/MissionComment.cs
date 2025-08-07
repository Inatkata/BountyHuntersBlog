using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BountyHuntersBlog.Constants.EntityConstants.MissionComment;
using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionComment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid MissionPostId { get; set; }

        public MissionPost MissionPost { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string HunterId { get; set; } = null!; 

        [ForeignKey(nameof(HunterId))]
        public Hunter Hunter { get; set; } = null!;

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}