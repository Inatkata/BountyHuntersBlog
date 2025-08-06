namespace BountyHuntersBlog.Models.Domain
{
    public class Faction
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;

        public ICollection<MissionPost> MissionPosts { get; set; } = new List<MissionPost>();
    }

}