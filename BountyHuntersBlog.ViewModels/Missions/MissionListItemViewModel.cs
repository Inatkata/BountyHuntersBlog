namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public IEnumerable<string> TagNames { get; set; } = Array.Empty<string>();
    }
}