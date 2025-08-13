// Services/Implementations/TagService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tags;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tags, IMapper mapper)
        {
            _tags = tags;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TagDto>> AllAsync() =>
            await _tags.AllReadonly()

                .OrderBy(t => t.Name)
                .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<TagDto?> GetAsync(int id)
        {
            var e = await _tags.GetByIdAsync(id);
            return e == null ? null : _mapper.Map<TagDto>(e);
        }

        public async Task<int> CreateAsync(TagDto dto)
        {
            var e = _mapper.Map<Tag>(dto);
            await _tags.AddAsync(e);
            await _tags.SaveChangesAsync();
            return e.Id;
        }

        public async Task<bool> UpdateAsync(TagDto dto)
        {
            var e = await _tags.GetByIdAsync(dto.Id);
            if (e == null) return false;
            e.Name = dto.Name;
            _tags.Update(e);
            await _tags.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var e = await _tags.GetByIdAsync(id);
            if (e == null) return false;
            e.IsDeleted = true;
            _tags.Update(e);
            await _tags.SaveChangesAsync();
            return true;
        }
    }
}