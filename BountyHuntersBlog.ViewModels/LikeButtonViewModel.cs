// ViewModels/LikeButtonViewModel.cs
namespace BountyHuntersBlog.ViewModels
{
    public class LikeButtonViewModel
    {
        // Новите полета за JSON
        public string TargetType { get; set; } = null!;  // "mission" | "comment"
        public int TargetId { get; set; }
        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }

        // Back-compat за стари Views
        public LikeTargetType Target { get; set; } = LikeTargetType.Mission;
        public bool IsAuthenticated { get; set; } = false;
        public bool IsLikedByCurrentUser { get; set; } = false;
        public int TotalCount { get => LikesCount; set => LikesCount = value; }
    }
}