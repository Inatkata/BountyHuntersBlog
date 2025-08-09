using System.Collections.Generic;
using System.Linq;
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

        // Pagination kept minimal; ideally push it down to repo
        public async Task<IEnumerable<MissionTagDto>> GetAllAsync(int page, int pageSize)
        {
            var all = await _repo.AllAsync(); // returns IEnumerable<MissionTag>
            var paged = all
                .Skip((page <= 0 ? 0 : (page - 1) * pageSize))
                .Take(pageSize > 0 ? pageSize : int.MaxValue);

            return _mapper.Map<IEnumerable<MissionTagDto>>(paged);
        }

        // Composite key
        public async Task<MissionTagDto?> GetAsync(int missionId, int tagId)
        {
            var entity = await _repo.GetAsync(missionId, tagId);
            return entity == null ? null : _mapper.Map<MissionTagDto>(entity);
        }

        public async Task CreateAsync(MissionTagDto dto)
        {
            // guard: avoid duplicates
            var exists = await _repo.ExistsAsync(dto.MissionId, dto.TagId);
            if (exists) return;

            var entity = _mapper.Map<MissionTag>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

        // Not meaningful for m2m; no-op by design
        public Task UpdateAsync(int missionId, int tagId, MissionTagDto dto)
            => Task.CompletedTask;

        public async Task DeleteAsync(int missionId, int tagId)
        {
            var entity = await _repo.GetAsync(missionId, tagId)
                         ?? throw new KeyNotFoundException("MissionTag not found");
            _repo.Delete(entity);
            await _repo.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(int missionId, int tagId)
            => _repo.ExistsAsync(missionId, tagId);

        // Helpers used by Missions Create/Edit screens
        public Task<IReadOnlyList<int>> GetTagIdsForMissionAsync(int missionId)
            => _repo.GetTagIdsForMissionAsync(missionId);

        public async Task SetMissionTagsAsync(int missionId, IEnumerable<int> tagIds)
        {
            await _repo.SetMissionTagsAsync(missionId, tagIds ?? Enumerable.Empty<int>());
            await _repo.SaveChangesAsync();
        }
    }
}
