using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repo;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> GetAllAsync(int page, int pageSize)
        {
            var entities = await _repo.AllAsync();
            // TODO: paging
            return _mapper.Map<IEnumerable<CommentDto>>(entities);
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null
                ? null
                : _mapper.Map<CommentDto>(entity);
        }

        public async Task CreateAsync(CommentDto dto)
        {
            var entity = _mapper.Map<Data.Models.Comment>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CommentDto dto)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"Comment {id} not found");
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"Comment {id} not found");
            _repo.Delete(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _repo.ExistsAsync(id);

        public async Task<IEnumerable<CommentDto>> GetCommentsByMissionIdAsync(int missionId)
        {
            var entities = await _repo.GetCommentsByMissionIdAsync(missionId);
            return _mapper.Map<IEnumerable<CommentDto>>(entities);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(string userId)
        {
            var entities = await _repo.GetCommentsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<CommentDto>>(entities);
        }
    }
}
