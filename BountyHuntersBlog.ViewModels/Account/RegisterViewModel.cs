// RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;
namespace BountyHuntersBlog.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, StringLength(50)]
        public string DisplayName { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}