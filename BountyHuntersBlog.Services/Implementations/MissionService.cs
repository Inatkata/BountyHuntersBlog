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
        public async Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(
            string? q, int? categoryId, int? tagId, int page, int pageSize)
        {
            var allEntities = await _missionRepo.AllAsync(); // returns IEnumerable<Mission>
            var query = allEntities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var qq = q.Trim().ToLower();
                query = query.Where(m =>
                    (m.Title ?? string.Empty).ToLower().Contains(qq) ||
                    (m.Description ?? string.Empty).ToLower().Contains(qq));
            }

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (tagId.HasValue && tagId.Value > 0)
                query = query.Where(m => m.MissionTags != null && m.MissionTags.Any(mt => mt.TagId == tagId.Value));

            var total = query.Count();

            var pageEntities = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pageDtos = _mapper.Map<List<MissionDto>>(pageEntities);
            return (pageDtos, total);
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
