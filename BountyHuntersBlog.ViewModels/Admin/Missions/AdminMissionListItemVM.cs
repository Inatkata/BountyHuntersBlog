// ViewModels/Admin/Missions/AdminMissionListItemVM.cs
using System;

namespace BountyHuntersBlog.ViewModels.Admin.Missions
{
    public class AdminMissionListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}