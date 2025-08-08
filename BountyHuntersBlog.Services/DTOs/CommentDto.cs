
namespace BountyHuntersBlog.Services.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int MissionId { get; set; }
        public string AuthorId { get; set; } = null!;
    }
}