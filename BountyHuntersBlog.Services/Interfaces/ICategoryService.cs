// Services/Interfaces/ICategoryService.cs
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> AllAsync();
        Task<CategoryDto?> GetAsync(int id);
        Task<int> CreateAsync(CategoryDto dto);
        Task<bool> UpdateAsync(CategoryDto dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}