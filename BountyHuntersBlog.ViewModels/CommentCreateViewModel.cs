namespace BountyHuntersBlog.ViewModels
{
    public class CommentCreateViewModel
    {
        public int MissionId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Content { get; set; } = null!;
    }
}