using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services.Implementations
{
    public class MissionService : IMissionService
    {
        private readonly IMissionRepository _missions;
        private readonly ICommentRepository _comments;
        private readonly ILikeRepository _likes;
        private readonly ICategoryRepository _categories;
        private readonly ITagRepository _tags;
        private readonly IMissionTagRepository _missionTags;

        public MissionService(
            IMissionRepository missions,
            ICommentRepository comments,
            ILikeRepository likes,
            ICategoryRepository categories,
            ITagRepository tags,
            IMissionTagRepository missionTags)
        {
            _missions = missions;
            _comments = comments;
            _likes = likes;
            _categories = categories;
            _tags = tags;
            _missionTags = missionTags;
        }

        private static string? GetUserId(ClaimsPrincipal user)
            => user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // SEARCH / LIST
        public async Task<(IReadOnlyList<MissionDto> items, int totalCount)> SearchPagedAsync(
            string? q, int? categoryId, int? tagId, int page, int pageSize)
        {
            // Use repository's SearchQueryable which already does ToLower().Contains() + Includes
            var baseQuery = _missions.SearchQueryable(q, categoryId, tagId);

            var total = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MissionDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    IsCompleted = m.IsCompleted,
                    CategoryName = m.Category != null ? m.Category.Name : "",
                    TagNames = m.MissionTags.Select(mt => mt.Tag.Name)
                })
                .ToListAsync();

            return (items, total);
        }

        // DETAILS
        public async Task<MissionDetailsDto?> GetDetailsAsync(int id)
        {
            var mission = await _missions.GetByIdWithIncludesAsync(id);
            if (mission == null) return null;

            return new MissionDetailsDto
            {
                Id = mission.Id,
                Title = mission.Title,
                Description = mission.Description,
                ImageUrl = mission.ImageUrl,
                CategoryName = mission.Category?.Name ?? "",
                TagNames = mission.MissionTags.Select(x => x.Tag.Name).ToList(),
                LikesCount = mission.Likes?.Count(l => l.MissionId == mission.Id) ?? 0,
                Comments = mission.Comments
                    .OrderByDescending(c => c.CreatedOn)
                    .Select(c => new MissionCommentDetailsDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedOn = c.CreatedOn,
                        UserDisplayName = c.User?.UserName ?? "User",
                        CanDelete = false // can be checked in controller if needed
                    })
                    .ToList()
            };
        }

        // LIKE (MISSION)
        public async Task ToggleMissionLikeAsync(int missionId, ClaimsPrincipal user)
        {
            var userId = GetUserId(user) ?? throw new InvalidOperationException("Unauthenticated user.");
            var mission = await _missions.GetByIdAsync(missionId)
                          ?? throw new KeyNotFoundException("Mission not found.");

            var existing = await _likes
                .All()
                .FirstOrDefaultAsync(l => l.MissionId == missionId && l.UserId == userId);

            if (existing != null)
            {
                _likes.Delete(existing);
            }
            else
            {
                await _likes.AddAsync(new Like
                {
                    UserId = userId,
                    MissionId = mission.Id,
                    CommentId = null
                });
            }

            await _likes.SaveChangesAsync();
        }

        // COMMENTS
        public async Task AddCommentAsync(int missionId, string content, ClaimsPrincipal user)
        {
            var userId = GetUserId(user) ?? throw new InvalidOperationException("Unauthenticated user.");
            _ = await _missions.GetByIdAsync(missionId) ?? throw new KeyNotFoundException("Mission not found.");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content is required.", nameof(content));

            await _comments.AddAsync(new Comment
            {
                MissionId = missionId,
                UserId = userId,
                Content = content.Trim(),
                CreatedOn = DateTime.UtcNow
            });

            await _comments.SaveChangesAsync();
        }

        public async Task ToggleCommentLikeAsync(int commentId, ClaimsPrincipal user)
        {
            var userId = GetUserId(user) ?? throw new InvalidOperationException("Unauthenticated user.");
            _ = await _comments.GetByIdAsync(commentId) ?? throw new KeyNotFoundException("Comment not found.");

            var existing = await _likes
                .All()
                .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);

            if (existing != null)
            {
                _likes.Delete(existing);
            }
            else
            {
                await _likes.AddAsync(new Like
                {
                    UserId = userId,
                    CommentId = commentId,
                    MissionId = null
                });
            }

            await _likes.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId, ClaimsPrincipal user)
        {
            var userId = GetUserId(user) ?? throw new InvalidOperationException("Unauthenticated user.");
            var comment = await _comments.All().FirstOrDefaultAsync(c => c.Id == commentId)
                          ?? throw new KeyNotFoundException("Comment not found.");

            var isOwner = comment.UserId == userId;
            var isAdmin = user.IsInRole("Admin") || user.IsInRole("Administrator");
            if (!isOwner && !isAdmin)
                throw new UnauthorizedAccessException("You cannot delete this comment.");

            _comments.Delete(comment);
            await _comments.SaveChangesAsync();
        }

        // EDIT
        public async Task<MissionEditDto?> GetEditAsync(int id)
        {
            var mission = await _missions.WithAllRelations()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null) return null;

            return new MissionEditDto
            {
                Id = mission.Id,
                Title = mission.Title,
                Description = mission.Description,
                ImageUrl = mission.ImageUrl,
                CategoryId = mission.CategoryId,
                TagIds = mission.MissionTags.Select(mt => mt.TagId).ToList(),
                IsCompleted = mission.IsCompleted
            };
        }

        public async Task EditAsync(MissionEditDto dto)
        {
            var mission = await _missions.WithAllRelations()
                              .FirstOrDefaultAsync(m => m.Id == dto.Id)
                          ?? throw new KeyNotFoundException("Mission not found.");

            // basic validation (optional but safe)
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new ArgumentException("Title is required.");
            if (string.IsNullOrWhiteSpace(dto.Description)) throw new ArgumentException("Description is required.");

            mission.Title = dto.Title.Trim();
            mission.Description = dto.Description.Trim();
            mission.ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim();
            mission.CategoryId = dto.CategoryId;
            mission.IsCompleted = dto.IsCompleted;

            var newTagIds = (dto.TagIds ?? Enumerable.Empty<int>()).Distinct().ToHashSet();

            // remove old
            var toRemove = mission.MissionTags.Where(mt => !newTagIds.Contains(mt.TagId)).ToList();
            foreach (var r in toRemove)
                _missionTags.Delete(r);

            // add new
            var existingTagIds = mission.MissionTags.Select(mt => mt.TagId).ToHashSet();
            var toAdd = newTagIds.Except(existingTagIds);
            foreach (var tagId in toAdd)
            {
                await _missionTags.AddAsync(new MissionTag { MissionId = mission.Id, TagId = tagId });
            }

            _missions.Update(mission);

            // IMPORTANT: persist changes (shared DbContext => един SaveChanges стига)
            await _missions.SaveChangesAsync();
        

        await _missions.SaveChangesAsync();
        }

        // SELECT LISTS
        public async Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync()
        {
            var items = await _categories.All()
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTagsSelectListAsync()
        {
            var items = await _tags.All()
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
                .ToListAsync();

            return items;
        }
        public async Task CreateAsync(MissionEditDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new ArgumentException("Title is required.");
            if (string.IsNullOrWhiteSpace(dto.Description)) throw new ArgumentException("Description is required.");

            var mission = new Mission
            {
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim(),
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim(),
                CategoryId = dto.CategoryId,
                IsCompleted = dto.IsCompleted,
                CreatedOn = DateTime.UtcNow
            };

            await _missions.AddAsync(mission);
            await _missions.SaveChangesAsync(); // need Id for tags

            var tagIds = (dto.TagIds ?? Enumerable.Empty<int>()).Distinct();
            foreach (var tagId in tagIds)
                await _missionTags.AddAsync(new MissionTag { MissionId = mission.Id, TagId = tagId });

            await _missionTags.SaveChangesAsync(); // или _missions.SaveChangesAsync() при общ DbContext
        }
        public async Task<MissionDto?> GetByIdAsync(int id)
        {
            var e = await _missions.GetByIdWithIncludesAsync(id);
            if (e == null) return null;
            return new MissionDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                ImageUrl = e.ImageUrl,
                CategoryId = e.CategoryId,
                IsDeleted = e.IsDeleted,
                IsCompleted = e.IsCompleted,
                CategoryName = e.Category?.Name ?? "",
                TagIds = e.MissionTags.Select(mt => mt.TagId).ToList(),
                TagNames = e.MissionTags.Select(mt => mt.Tag.Name).ToList()
            };
        }

        public async Task UpdateAsync(MissionDto dto)
        {
            await EditAsync(new MissionEditDto
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                TagIds = dto.TagIds,
                IsCompleted = dto.IsCompleted
            });
        }

        public async Task SoftDeleteAsync(int id)
        {
            var e = await _missions.GetByIdAsync(id) ?? throw new KeyNotFoundException();
            e.IsDeleted = true;
            _missions.Update(e);
            await _missions.SaveChangesAsync();
        }

    }
}
