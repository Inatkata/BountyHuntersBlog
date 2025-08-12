// ViewModels/Admin/Users/AdminUserListItemVM.cs
using System.Collections.Generic;

namespace BountyHuntersBlog.ViewModels.Admin.Users
{
    public class AdminUserListItemVM
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public int MissionsCount { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public bool IsLockedOut { get; set; }
    }
}