
using System.ComponentModel.DataAnnotations;
using static BountyHuntersBlog.Constants.EntityConstants.Hunter;
namespace BountyHuntersBlog.Models.Domain
{
    using Microsoft.AspNetCore.Identity;

    using Microsoft.AspNetCore.Identity;

    public class Hunter : IdentityUser
    {
        [Required]
        [MaxLength(DisplayNameMaxLength)]
        public string DisplayName { get; set; } = null!;

        public ICollection<MissionPost> MissionPosts { get; set; } = new List<MissionPost>();
        public ICollection<MissionLike> Likes { get; set; } = new List<MissionLike>();
        public ICollection<MissionComment> Comments { get; set; } = new List<MissionComment>();
    }



}
