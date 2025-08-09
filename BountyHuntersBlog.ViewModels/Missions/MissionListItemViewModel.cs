namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public int LikesCount { get; set; }
    }
}