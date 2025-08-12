// Services/Implementations/MissionService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Services.Implementations
{
    public class MissionService : IMissionService
    {
        private readonly IMissionRepository _missions;
        private readonly IMissionTagRepository _missionTags;
        private readonly ILikeRepository _likes;
        private readonly ICommentRepository _comments;
        private readonly IMapper _mapper;

        public MissionService(IMissionRepository missions, IMissionTagRepository missionTags,
                              ILikeRepository likes, ICommentRepository comments, IMapper mapper)
        {
            _missions = missions;
            _missionTags = missionTags;
            _likes = likes;
            _comments = comments;
            _mapper = mapper;
        }

        public async Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(
            string? q, int? categoryId, int? tagId, int page, int pageSize)
        {
            var query = _missions.SearchQueryable(q, categoryId, tagId)
                                 .OrderByDescending(m => m.CreatedOn);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ProjectTo<MissionDto>(_mapper.ConfigurationProvider)
                                   .ToListAsync();

            return (items, total);
        }

        public async Task<MissionDto?> GetByIdAsync(int id)
        {
            var entity = await _missions.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<MissionDto>(entity);
        }

        public async Task<MissionWithStatsDto?> GetDetailsAsync(int id)
        {
            var entity = await _missions.GetByIdWithIncludesAsync(id);
            if (entity == null) return null;

            var dto = _mapper.Map<MissionWithStatsDto>(entity);
            dto.LikesCount = await _likes.AllAsQueryable().CountAsync(l => l.MissionId == id);
            dto.CommentsCount = await _comments.AllAsQueryable().CountAsync(c => c.MissionId == id);
            return dto;
        }

        // Services/Implementations/MissionService.cs
        public async Task<int> CreateAsync(MissionDto dto)
        {
            var entity = _mapper.Map<Mission>(dto);
            entity.CreatedOn = DateTime.UtcNow;

            // attach category
            entity.CategoryId = dto.CategoryId;

            // mission tags
            entity.MissionTags = dto.TagIds?.Distinct()
                .Select(tid => new MissionTag { TagId = tid })
                .ToList() ?? new List<MissionTag>();

            // author
            if (!string.IsNullOrEmpty(dto.UserId))
                entity.UserId = dto.UserId;

            await _missions.AddAsync(entity);
            await _missions.SaveChangesAsync();

            return entity.Id;
        }



        public async Task<bool> UpdateAsync(MissionDto dto)
        {
            var entity = await _missions.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            // simple fields
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.ImageUrl = dto.ImageUrl;
            entity.IsCompleted = dto.IsCompleted;
            entity.IsDeleted = dto.IsDeleted;
            entity.CategoryId = dto.CategoryId;

            // sync MissionTags (remove missing; add new)
            var newSet = (dto.TagIds ?? Enumerable.Empty<int>()).Distinct().ToHashSet();
            var currentSet = entity.MissionTags?.Select(mt => mt.TagId).ToHashSet() ?? new HashSet<int>();

            // remove
            foreach (var mt in entity.MissionTags.Where(mt => !newSet.Contains(mt.TagId)).ToList())
                entity.MissionTags.Remove(mt);

            // add
            foreach (var tid in newSet.Except(currentSet))
                entity.MissionTags.Add(new MissionTag { MissionId = entity.Id, TagId = tid });

            await _missions.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _missions.GetByIdAsync(id);
            if (entity == null) return false;
            entity.IsDeleted = true;
            _missions.Update(entity);
            await _missions.SaveChangesAsync();
            return true;
        }
    }
}
