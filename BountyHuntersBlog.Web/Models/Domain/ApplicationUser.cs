using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BountyHuntersBlog.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public bool IsApplicationUser { get; set; } = false;

        public ICollection<MissionPost> PostedMissions { get; set; } = new List<MissionPost>();
        public ICollection<MissionPost> TakenMissions { get; set; } = new List<MissionPost>();
        public ICollection<MissionComment> Comments { get; set; } = new List<MissionComment>();
        public ICollection<MissionLike> Likes { get; set; } = new List<MissionLike>();
    }
}