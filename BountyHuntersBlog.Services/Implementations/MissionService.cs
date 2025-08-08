using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services.Implementations
{
    public class MissionService : IMissionService
    {
        private readonly IMissionRepository _missionRepo;
        private readonly IMapper _mapper;

        public MissionService(IMissionRepository missionRepo, IMapper mapper)
        {
            _missionRepo = missionRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MissionDto>> GetAllAsync(int page, int pageSize)
        {
            var entities = await _missionRepo.AllAsync();
            // тук може да приложиш paging върху entities
            return _mapper.Map<IEnumerable<MissionDto>>(entities);
        }

        public async Task<MissionDto?> GetByIdAsync(int id)
        {
            var mission = await _missionRepo.GetByIdAsync(id);
            return mission is null ? null : _mapper.Map<MissionDto>(mission);
        }

        public async Task CreateAsync(MissionDto dto)
        {
            var entity = _mapper.Map<Mission>(dto);
            await _missionRepo.AddAsync(entity);
            await _missionRepo.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, MissionDto dto)
        {
            var mission = await _missionRepo.GetByIdAsync(id);
            if (mission is null) throw new KeyNotFoundException();
            _mapper.Map(dto, mission);
            _missionRepo.Update(mission);
            await _missionRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mission = await _missionRepo.GetByIdAsync(id);
            if (mission is null) throw new KeyNotFoundException();
            _missionRepo.Delete(mission);
            await _missionRepo.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _missionRepo.ExistsAsync(id);
    }
}
