namespace BountyHuntersBlog.Models.Requests
{
    public class AddLikeRequest
    {
        public Guid MissionPostId { get; set; }
        public string ApplicationUserId { get; set; }
    }
}