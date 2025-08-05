
using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Models.Domain
{
    public class Hunter : IdentityUser
    {
        public string DisplayName { get; set; }

        public ICollection<MissionPost> Missions { get; set; } =new List<MissionPost>();


    }

}
