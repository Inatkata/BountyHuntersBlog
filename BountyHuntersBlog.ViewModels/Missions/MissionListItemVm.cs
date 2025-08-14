// BountyHuntersBlog.ViewModels/Missions/MissionListItemVm.cs
namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionListItemVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string CategoryName { get; set; } = "";
        public IEnumerable<string>? TagNames { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ShortDescription { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        // Optional: public string? ImageUrl { get; set; }
    }
}