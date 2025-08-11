using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BountyHuntersBlog.Services.Implementations
{
    public class MissionService : IMissionService
    {
        private readonly IMissionRepository _missions;
        private readonly ILikeRepository _likes;
        private readonly ICommentRepository _comments;
        private readonly ITagRepository _tags;
        private readonly IMapper _mapper;

        public MissionService(
            IMissionRepository missions,
            ILikeRepository likes,
            ICommentRepository comments,
            ITagRepository tags,
            IMapper mapper)
        {
            _missions = missions;
            _likes = likes;
            _comments = comments;
            _tags = tags;
            _mapper = mapper;
        }

        public async Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(
            string? q, int? categoryId, int? tagId, int page, int pageSize)
        {
            var query = (await _missions.AllAsync()).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var qq = q.Trim().ToLower();
                query = query.Where(m => m.Title.ToLower().Contains(qq) || m.Description.ToLower().Contains(qq));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(m => m.CategoryId == categoryId.Value);
            }

            if (tagId.HasValue)
            {
                query = query.Where(m => m.MissionTags.Any(mt => mt.TagId == tagId.Value));
            }

            var total = query.Count();

            var pageItems = query
                .OrderByDescending(m => m.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<IReadOnlyList<MissionDto>>(pageItems);
            return (dtos, total);
        }

        public async Task<MissionDto?> GetByIdAsync(int id)
        {
            var entity = await _missions.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<MissionDto>(entity);
        }

        public async Task<MissionWithStatsDto?> GetByIdWithStatsAsync(int id, ClaimsPrincipal? user = null)
        {
            var entity =
                await _missions
                    .GetByIdWithIncludesAsync(id); // include Category, MissionTags->Tag, User, Comments->User, Likes
            if (entity == null) return null;

            var currentUserId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

            var dto = new MissionWithStatsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                UserDisplayName = entity.User?.DisplayName ?? entity.User?.UserName,
                CreatedOnUtc = entity.CreatedOn,
                TagNames = entity.MissionTags.Select(mt => mt.Tag.Name).Distinct().ToList(),
                LikesCount = entity.Likes.Count,
                LikedByCurrentUser = !string.IsNullOrEmpty(currentUserId) &&
                                     entity.Likes.Any(l => l.UserId == currentUserId),
                Comments = entity.Comments
                    .OrderByDescending(c => c.CreatedOn)
                    .Select(c => new MissionWithStatsDto.CommentItem
                    {
                        Id = c.Id,
                        Text = c.Text,
                        UserUserName = c.User?.UserName,
                        UserDisplayName = c.User?.DisplayName ?? c.User?.UserName,
                        CreatedOnUtc = c.CreatedOn,
                        LikesCount = c.Likes.Count,
                        LikedByCurrentUser = !string.IsNullOrEmpty(currentUserId) &&
                                             c.Likes.Any(l => l.UserId == currentUserId)
                    }).ToList()
            };

            return dto;
        }

        public async Task CreateAsync(MissionDto dto, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                         throw new InvalidOperationException("Unauthenticated.");
            var entity = _mapper.Map<Mission>(dto);
            entity.UserId = userId;
            entity.CreatedOn = DateTime.UtcNow;

            // tags
            entity.MissionTags = dto.TagIds?.Distinct().Select(tid => new MissionTag { TagId = tid }).ToList() ??
                                 new List<MissionTag>();

            await _missions.AddAsync(entity);
            await _missions.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, MissionDto dto, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                         throw new InvalidOperationException("Unauthenticated.");
            var entity = await _missions.GetByIdWithIncludesAsync(id);
            if (entity == null) throw new KeyNotFoundException("Mission not found.");
            if (entity.UserId != userId && !(user.IsInRole("Admin")))
                throw new UnauthorizedAccessException("Not allowed.");

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.CategoryId = dto.CategoryId;

            // reset tags
            entity.MissionTags.Clear();
            foreach (var tid in dto.TagIds ?? Enumerable.Empty<int>())
                entity.MissionTags.Add(new MissionTag { MissionId = id, TagId = tid });

            await _missions.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _missions.GetByIdWithIncludesAsync(id)
                         ?? throw new KeyNotFoundException("Mission not found.");
            _missions.Delete(entity);
            await _missions.SaveChangesAsync();
        }
    }
}
