namespace BountyHuntersBlog.Services.DTOs
{
    public class MissionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string CategoryName { get; set; } = "";
        public IEnumerable<string> TagNames { get; set; } = new List<string>();
    }

    public class MissionDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = "";
        public IEnumerable<string> TagNames { get; set; } = new List<string>();
        public int LikesCount { get; set; }
        public List<MissionCommentDetailsDto> Comments { get; set; } = new();
    }

    public class MissionCommentDetailsDto
    {
        public int Id { get; set; }
        public string UserDisplayName { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; } = "";
        public bool CanDelete { get; set; }
    }

    public class MissionEditDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<int> TagIds { get; set; } = new List<int>();
        public bool IsCompleted { get; set; }
    }
}