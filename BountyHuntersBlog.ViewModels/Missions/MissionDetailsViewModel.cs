// BountyHuntersBlog.ViewModels/Missions/MissionDetailsViewModel.cs
namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = "";
        public IEnumerable<string>? TagNames { get; set; }
        public int LikesCount { get; set; }
        public List<MissionCommentVm> Comments { get; set; } = new();
    }
}