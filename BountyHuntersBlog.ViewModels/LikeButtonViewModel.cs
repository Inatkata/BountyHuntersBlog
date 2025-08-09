namespace BountyHuntersBlog.ViewModels
{
    public enum LikeTargetType { Mission, Comment }

    public class LikeButtonViewModel
    {
        public LikeTargetType TargetType { get; set; }
        public int TargetId { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public int TotalCount { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}