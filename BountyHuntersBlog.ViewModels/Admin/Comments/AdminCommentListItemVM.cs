
using System;

namespace BountyHuntersBlog.ViewModels.Admin.Comments
{
    public class AdminCommentListItemVM
    {
        public int Id { get; set; }
        public string MissionTitle { get; set; } = null!;
        public string UserDisplayName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int LikesCount { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}