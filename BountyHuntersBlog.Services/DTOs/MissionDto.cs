// Services/DTOs/MissionDto.cs
namespace BountyHuntersBlog.Services.DTOs
{
    public class MissionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; }

        public string UserId { get; set; } = null!;
        public int CategoryId { get; set; }

        public IEnumerable<int> TagIds { get; set; } = new List<int>();
    }
}