
namespace BountyHuntersBlog.Models.Domain
{
    public class MissionLike
    {
        public Guid MissionPostId { get; set; }
        public string UserId { get; set; }

        public MissionPost MissionPost { get; set; }
    }
}
