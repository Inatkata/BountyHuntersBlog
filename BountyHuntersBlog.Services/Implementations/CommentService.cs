using AutoMapper;
using AutoMapper.QueryableExtensions;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BountyHuntersBlog.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _comments;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository comments, IMapper mapper)
        {
            _comments = comments;
            _mapper = mapper;
        }

        public async Task<CommentDto> AddAsync(int missionId, string userId, string content)
        {
            var entity = new Comment
            {
                MissionId = missionId,
                UserId = userId,
                Content = content
            };

            await _comments.AddAsync(entity);
            await _comments.SaveChangesAsync();

            var withIncludes = await _comments.GetByIdWithIncludesAsync(entity.Id);
            return _mapper.Map<CommentDto>(withIncludes ?? entity);
        }

        public async Task<bool> EditAsync(int id, string content)
        {
            var entity = await _comments.GetByIdAsync(id);
            if (entity == null) return false;
            entity.Content = content;
            _comments.Update(entity);
            await _comments.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _comments.GetByIdAsync(id);
            if (entity == null) return false;
            _comments.Delete(entity);
            await _comments.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<CommentDto>> GetForMissionAsync(int missionId)
        {
            var query = _comments.AllAsQueryable()
                .Where(c => c.MissionId == missionId)
                .OrderByDescending(c => c.CreatedOn);
            return await query.ProjectTo<CommentDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var entity = await _comments.GetByIdWithIncludesAsync(id);
            return entity == null ? null : _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto> SoftDeleteAsync(int id)
        {
            var entity = await _comments.GetByIdAsync(id);
            if (entity == null) throw new ArgumentException("Comment not found", nameof(id));
            entity.IsDeleted = true;
            _comments.Update(entity);
            await _comments.SaveChangesAsync();
            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<IReadOnlyList<CommentDto>> AllAsync()
        {
            var query = _comments.AllAsQueryable()          // IQueryable<Comment>
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedOn);

            return await query
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

    }
}
