// ViewModels/Admin/Categories/AdminCategoryFormVM.cs
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Admin.Categories
{
    public class AdminCategoryFormVM
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}