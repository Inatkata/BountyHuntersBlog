
namespace BountyHuntersBlog.Models.Domain
{
    using Microsoft.AspNetCore.Identity;

    using Microsoft.AspNetCore.Identity;

    public class Hunter : IdentityUser
    {
        public string DisplayName { get; set; } = null!;

        public ICollection<MissionPost> MissionPosts { get; set; } = new List<MissionPost>();
        public ICollection<MissionLike> Likes { get; set; } = new List<MissionLike>();
        public ICollection<MissionComment> Comments { get; set; } = new List<MissionComment>();
    }



}
