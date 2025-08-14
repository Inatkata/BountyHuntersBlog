// LoginViewModel.cs
using System.ComponentModel.DataAnnotations;
namespace BountyHuntersBlog.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required, StringLength(50)]
        public string Username { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}