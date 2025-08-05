namespace BountyHuntersBlog.Models.Domain
{
    public class Faction
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public ICollection<MissionPost> MissionPosts { get; set; }
    }
}