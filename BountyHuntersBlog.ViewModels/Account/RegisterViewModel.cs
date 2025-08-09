using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}