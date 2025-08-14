using System.ComponentModel.DataAnnotations;

public class AdminCategoryFormVM
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
}