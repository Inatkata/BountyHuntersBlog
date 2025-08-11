// Services/Interfaces/ITagService.cs
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ITagService
    {
        Task<IReadOnlyList<TagDto>> AllAsync();
        Task<TagDto?> GetAsync(int id);
        Task<int> CreateAsync(TagDto dto);
        Task<bool> UpdateAsync(TagDto dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}