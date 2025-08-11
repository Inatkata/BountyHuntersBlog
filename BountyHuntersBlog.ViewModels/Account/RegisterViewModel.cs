using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Account
{
    public class RegisterViewModel
    {
        // Някои твои изгледи искат Username (малко 'n').
        // Оставям и двете, за да няма счупвания.
        [Required, StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = null!;

        public string? Username   // alias към UserName
        {
            get => UserName;
            set { if (!string.IsNullOrWhiteSpace(value)) UserName = value!; }
        }

        [Required, StringLength(50, MinimumLength = 2)]
        public string DisplayName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}