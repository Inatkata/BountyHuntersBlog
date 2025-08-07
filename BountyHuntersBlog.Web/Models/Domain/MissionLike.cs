using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Domain
{
    public class MissionLike
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MissionPostId { get; set; }
        public MissionPost MissionPost { get; set; }

        public Guid HunterId { get; set; }
        public Hunter Hunter { get; set; }
    }
}