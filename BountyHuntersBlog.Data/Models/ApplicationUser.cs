using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(ModelConstants.User.DisplayNameMaxLength)]
        public string? DisplayName { get; set; } = "Anonymous";

        public ICollection<Mission> Missions { get; set; } = new HashSet<Mission>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
    }
}