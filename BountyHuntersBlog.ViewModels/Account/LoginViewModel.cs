namespace BountyHuntersBlog.ViewModels.Account
{
    public class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Email { get; set; } = null!;   

        [System.ComponentModel.DataAnnotations.Required,
         System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }   

        public string? ReturnUrl { get; set; }
    }
}