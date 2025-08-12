// ViewModels/Admin/Tags/AdminTagFormVM.cs
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Admin.Tags
{
    public class AdminTagFormVM
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}