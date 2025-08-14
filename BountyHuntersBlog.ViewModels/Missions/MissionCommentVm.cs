// BountyHuntersBlog.ViewModels/Missions/MissionCommentVm.cs
namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionCommentVm
    {
        public int Id { get; set; }
        public string UserDisplayName { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; } = "";
        public bool CanDelete { get; set; }
    }
}