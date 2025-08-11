namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; } = new();

        // display-only
        public string? CategoryName { get; set; }
        public List<string> TagNames { get; set; } = new();

        // за вюта, които ги показват
        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;

        public string? UserName { get; set; }
    }
}