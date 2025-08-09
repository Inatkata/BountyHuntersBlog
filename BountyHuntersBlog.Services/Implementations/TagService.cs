using AutoMapper;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync(int page, int pageSize)
        {
            var entities = await _repo.AllAsync(); // TODO: добави реален paging ако искаш
            return _mapper.Map<IEnumerable<TagDto>>(entities);
        }

        public async Task<TagDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<TagDto>(entity);
        }

        public async Task CreateAsync(TagDto dto)
        {
            var entity = _mapper.Map<Tag>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TagDto dto)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"Tag {id} not found");
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"Tag {id} not found");
            _repo.Delete(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) => await _repo.ExistsAsync(id);
    }
}