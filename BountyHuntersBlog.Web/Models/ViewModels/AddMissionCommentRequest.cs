using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class AddMissionCommentRequest
    {
        [Required]
        public string Content { get; set; }

        public Guid MissionPostId { get; set; }

        public Guid HunterId { get; set; }
    }
}