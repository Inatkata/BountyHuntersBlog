using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.ViewModels.Comments
{
    public class EditCommentRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.Comment.ContentMaxLength)]
        public string Content { get; set; } = null!;
    }
}