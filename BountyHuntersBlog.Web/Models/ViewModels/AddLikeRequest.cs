namespace BountyHuntersBlog.Models.ViewModels
{
    public class AddLikeRequest
    {
        public Guid MissionPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
