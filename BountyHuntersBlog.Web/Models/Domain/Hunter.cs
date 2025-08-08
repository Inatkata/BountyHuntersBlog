using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Models.Domain
{
    public class Hunter : IdentityUser
    {
        public string DisplayName { get; set; }

        // Примерни релации (добави или махни според нуждите на проекта)
        public ICollection<MissionPost> MissionPosts { get; set; }
        public ICollection<MissionLike> MissionLikes { get; set; }
        public ICollection<MissionComment> MissionComments { get; set; }
    }
}