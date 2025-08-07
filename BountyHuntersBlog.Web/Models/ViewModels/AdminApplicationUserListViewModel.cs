using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class AdminApplicationUserListViewModel
    {
        public List<ApplicationUser> ApplicationUsers { get; set; } = new();

        public CreateApplicationUserViewModel CreateApplicationUser { get; set; } = new();
    }
}