// Services/DTOs/CommentDto.cs
namespace BountyHuntersBlog.Services.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string UserId { get; set; } = null!;
        public string UserDisplayName { get; set; } = null!;

        public int MissionId { get; set; }
    }
}