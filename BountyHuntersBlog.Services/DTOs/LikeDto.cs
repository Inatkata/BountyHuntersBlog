namespace BountyHuntersBlog.Services.DTOs
{
    public class LikeDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int? MissionId { get; set; }
        public int? CommentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}