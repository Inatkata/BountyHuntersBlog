using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionLike
    {
        public Guid MissionPostId { get; set; }
        public MissionPost MissionPost { get; set; }

        [Required]
        public string HunterId { get; set; }

        [ForeignKey("HunterId")]
        public Hunter Hunter { get; set; }
    }
}