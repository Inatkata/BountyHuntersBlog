using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Auth
{
    public class LoginVM
    {
        [Required] public string UserName { get; set; } = "";
        [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }

    
}