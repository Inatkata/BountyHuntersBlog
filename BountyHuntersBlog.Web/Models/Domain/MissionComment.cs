using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionComment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public Guid MissionPostId { get; set; }
        public MissionPost MissionPost { get; set; }

        [Required]
        public string HunterId { get; set; }
        public Hunter Hunter { get; set; }
    }

}