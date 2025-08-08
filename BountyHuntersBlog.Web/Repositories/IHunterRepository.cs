using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IHunterRepository
    {
        Task<IEnumerable<Hunter>> GetAllAsync();
        Task<Hunter?> GetAsync(Guid id);
        Task<Hunter> AddAsync(Hunter hunter);
        Task<Hunter?> UpdateAsync(Hunter hunter);
        Task<Hunter?> DeleteAsync(Guid id);
    }
}