namespace BountyHuntersBlog.ViewModels.Admin.Comments
{
    public class AdminCommentListItemVM
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string MissionTitle { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int LikesCount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}