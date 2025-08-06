

using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IFactionRepository
    {
        Task<IEnumerable<Faction>> GetAllAsync(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNumber,
            int pageSize);

        Task<int> CountAsync(string? searchQuery);


        Task<Faction?> GetAsync(Guid id);
        Task<Faction> AddAsync(Faction faction);
        Task<Faction?> UpdateAsync(Faction faction);
        Task<Faction?> DeleteAsync(Guid id);
    }
}
