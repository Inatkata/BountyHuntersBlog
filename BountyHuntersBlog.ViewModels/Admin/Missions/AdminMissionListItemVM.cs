// BountyHuntersBlog.ViewModels/Admin/Missions/AdminMissionListItemVM.cs
namespace BountyHuntersBlog.ViewModels.Admin.Missions
{
    public class AdminMissionListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CategoryName { get; set; } = "";
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
    }
}