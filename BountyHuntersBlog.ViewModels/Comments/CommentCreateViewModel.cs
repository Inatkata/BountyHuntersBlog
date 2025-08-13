using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.ViewModels.Comments
{
    public class CommentCreateViewModel
    {
        public int MissionId { get; set; }
        [Required]
        [MaxLength(ModelConstants.Comment.ContentMaxLength)]
        public string Content { get; set; } = null!;
    }
}