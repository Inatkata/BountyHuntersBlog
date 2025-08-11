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

        public async Task<int> CreateAsync(MissionDto dto)
        {
            var entity = _mapper.Map<Mission>(dto);
            await _missions.AddAsync(entity);
            await _missions.SaveChangesAsync();

            if (dto.TagIds != null && dto.TagIds.Any())
            {
                foreach (var tagId in dto.TagIds.Distinct())
                {
                    if (!await _missionTags.LinkExistsAsync(entity.Id, tagId))
                        await _missionTags.AddAsync(new MissionTag { MissionId = entity.Id, TagId = tagId });
                }
                await _missionTags.SaveChangesAsync();
            }
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(MissionDto dto)
        {
            var entity = await _missions.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.CategoryId = dto.CategoryId;

            _missions.Update(entity);
            await _missions.SaveChangesAsync();

            // sync tags
            var newTags = (dto.TagIds ?? Enumerable.Empty<int>()).ToHashSet();
            var existing = _missionTags.AllAsQueryable()
                                       .Where(x => x.MissionId == entity.Id);
            var existingList = await existing.ToListAsync();

            // remove missing
            foreach (var mt in existingList.Where(x => !newTags.Contains(x.TagId)))
                _missionTags.Delete(mt);

            // add new
            foreach (var tagId in newTags.Where(t => !existingList.Any(x => x.TagId == t)))
                await _missionTags.AddAsync(new MissionTag { MissionId = entity.Id, TagId = tagId });

            await _missionTags.SaveChangesAsync();
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
