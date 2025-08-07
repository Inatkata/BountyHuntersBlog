using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionComment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        // Foreign Keys
        public Guid MissionPostId { get; set; }
        public MissionPost MissionPost { get; set; }

        public Guid HunterId { get; set; }
        public Hunter Hunter { get; set; }
    }
}