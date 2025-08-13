// Services/DTOs/MissionWithStatsDto.cs
namespace BountyHuntersBlog.Services.DTOs
{
    public class MissionWithStatsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public IEnumerable<string> TagNames { get; set; } = new List<string>();

        public bool IsCompleted { get; set; }

        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
    }
}