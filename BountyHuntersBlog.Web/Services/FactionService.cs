using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services
{
    public class FactionService : IFactionService
    {
        private readonly IFactionRepository _factionRepo;

        public FactionService(IFactionRepository factionRepo)
        {
            _factionRepo = factionRepo;
        }

        public async Task<IEnumerable<Faction>> GetAllAsync()
        {
            return await _factionRepo.GetAllAsync(); 
        }


        public async Task<Faction?> GetByIdAsync(Guid id)
        {
            return await _factionRepo.GetAsync(id);
        }

        public async Task AddAsync(Faction faction)
        {
            await _factionRepo.AddAsync(faction);
        }

        public async Task UpdateAsync(Faction faction)
        {
            await _factionRepo.UpdateAsync(faction);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _factionRepo.DeleteAsync(id);
        }
    }
}
