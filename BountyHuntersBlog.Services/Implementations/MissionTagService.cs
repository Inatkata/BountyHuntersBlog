using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services.Implementations
{
    public class MissionTagService : IMissionTagService
    {
        private readonly IMissionTagRepository _repo;
        private readonly IMapper _mapper;

        public MissionTagService(IMissionTagRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MissionTagDto>> GetAllAsync(int page, int pageSize)
        {
            var entities = await _repo.AllAsync();
            return _mapper.Map<IEnumerable<MissionTagDto>>(entities);
        }

        public async Task<MissionTagDto?> GetByIdAsync(int id)
        {
            // composite key – може да не реализираме GetById
            return null;
        }

        public async Task CreateAsync(MissionTagDto dto)
        {
            var entity = _mapper.Map<MissionTag>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, MissionTagDto dto)
        {
            // няма смисъл – many-to-many се управлява при създаване/триене
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"MissionTag not found");
            _repo.Delete(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _repo.GetByIdAsync(id) != null;
    }
}