
using Microsoft.AspNetCore.Identity;

namespace BountyHuntersBlog.Models.Domain
{
    using Microsoft.AspNetCore.Identity;

    public class Hunter : IdentityUser
    {
        public string DisplayName { get; set; }

        public ICollection<MissionPost> Missions { get; set; } = new List<MissionPost>();
    }


}
