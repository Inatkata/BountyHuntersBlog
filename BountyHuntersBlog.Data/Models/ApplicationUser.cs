

using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Mission> Missions { get; set; } = new List<Mission>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}