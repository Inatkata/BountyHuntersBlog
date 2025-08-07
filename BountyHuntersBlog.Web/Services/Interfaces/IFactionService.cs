using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IFactionService
    {
        Task<IEnumerable<Faction>> GetAllAsync();
        Task<Faction?> GetByIdAsync(Guid id);
        Task AddAsync(Faction faction);
        Task UpdateAsync(Faction faction);
        Task DeleteAsync(Guid id);
    }
}
