// Services/DTOs/LikeResultDto.cs
namespace BountyHuntersBlog.Services.DTOs
{
    public class LikeResultDto
    {
        public string TargetType { get; set; } = null!;
        public int TargetId { get; set; }
        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }
    }
}