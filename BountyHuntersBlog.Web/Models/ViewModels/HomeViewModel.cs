using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<MissionPost> MissionPosts { get; set; }

        public IEnumerable<Faction> Factions { get; set; }
    }
}
