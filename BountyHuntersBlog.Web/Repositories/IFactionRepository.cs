using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Repositories
{
    public interface IFactionRepository
    {
        Task<IEnumerable<Faction>> GetAllAsync();
        Task<IEnumerable<Faction>> GetSelectedFactionsAsync(List<Guid> selectedFactionIds);
        Task<Faction?> GetByIdAsync(Guid id);
        Task<Faction> AddAsync(Faction faction);
        Task<Faction?> UpdateAsync(Faction faction);
        Task<Faction?> DeleteAsync(Guid id);
    }
}