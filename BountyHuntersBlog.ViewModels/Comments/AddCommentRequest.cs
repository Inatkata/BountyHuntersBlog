using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.ViewModels.Comments
{
    public class AddCommentRequest
    {
        [Required]
        public int MissionId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        [MaxLength(ModelConstants.Comment.ContentMaxLength)]
        public string Content { get; set; } = null!;
    }
}