
namespace BountyHuntersBlog.Models.Domain
{
    public class MissionComment
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public string UserId { get; set; }
        public Hunter User { get; set; }

        public Guid MissionPostId { get; set; }
        public MissionPost MissionPost { get; set; }
    }
}
