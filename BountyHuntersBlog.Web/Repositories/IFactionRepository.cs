using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IFactionRepository
    {
        Task<IEnumerable<Faction>> GetAllAsync();
        Task<Faction?> GetAsync(Guid id);
        Task<Faction> AddAsync(Faction faction);
        Task<Faction?> UpdateAsync(Faction faction);
        Task<Faction?> DeleteAsync(Guid id);
        Task<Faction?> GetByIdAsync(Guid id);

    }
}