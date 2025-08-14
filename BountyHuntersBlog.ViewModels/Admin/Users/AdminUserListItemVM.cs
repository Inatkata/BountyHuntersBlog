namespace BountyHuntersBlog.ViewModels.Admin.Users
{
    public class AdminUserListItemVM
    {
        public string Id { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public List<string> Roles { get; set; } = new();
        public bool IsLockedOut { get; set; }
    }
}