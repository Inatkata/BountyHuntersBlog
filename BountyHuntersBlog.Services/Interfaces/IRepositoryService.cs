
namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IRepositoryService<TDto>
        where TDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync(int page, int pageSize);
        Task<TDto?> GetByIdAsync(int id);
        Task CreateAsync(TDto dto);
        Task UpdateAsync(int id, TDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}