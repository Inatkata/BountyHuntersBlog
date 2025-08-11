namespace BountyHuntersBlog.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string UserName { get; set; } = null!;

        public int MissionId { get; set; }

        public string MissionTitle { get; set; } = null!;
    }
}