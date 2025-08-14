using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Admin.Users
{
    public class AdminUserRolesVM
    {
        [Required]
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";

        public List<string> AvailableRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
    }
}