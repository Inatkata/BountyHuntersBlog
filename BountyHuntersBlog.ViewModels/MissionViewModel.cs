namespace BountyHuntersBlog.ViewModels
{
    public class MissionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool IsCompleted { get; set; }

        public string AuthorName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;

        public IEnumerable<string> TagNames { get; set; } = new List<string>();
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
    }
}