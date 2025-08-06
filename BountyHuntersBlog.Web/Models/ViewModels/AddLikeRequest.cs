namespace BountyHuntersBlog.Models.ViewModels
{
    public class AddLikeRequest
    {
        public Guid MissionPostId { get; set; }
        public string UserId { get; set; }

    }
}
