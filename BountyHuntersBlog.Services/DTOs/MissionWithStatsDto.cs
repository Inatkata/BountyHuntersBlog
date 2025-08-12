// Services/DTOs/MissionWithStatsDto.cs
namespace BountyHuntersBlog.Services.DTOs
{
    public class MissionWithStatsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
        public bool IsCompleted { get; set; }

        public string UserId { get; set; } = null!;
        public string UserDisplayName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public IEnumerable<int> TagIds { get; set; } = new List<int>();
        public IEnumerable<TagDto> Tags { get; set; } = new List<TagDto>();

        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}