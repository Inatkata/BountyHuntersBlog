using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        public string UserNameOrEmail { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}