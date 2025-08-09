namespace BountyHuntersBlog.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string AuthorName { get; set; } = null!;
    }
}