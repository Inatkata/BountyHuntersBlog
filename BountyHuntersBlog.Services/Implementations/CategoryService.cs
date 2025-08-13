// Services/Implementations/CategoryService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BountyHuntersBlog.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categories;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categories, IMapper mapper)
        {
            _categories = categories;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CategoryDto>> AllAsync() =>
            await _categories.AllReadonly()

                .OrderBy(c => c.Name)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<CategoryDto?> GetAsync(int id)
        {
            var e = await _categories.GetByIdAsync(id);
            return e == null ? null : _mapper.Map<CategoryDto>(e);
        }

        public async Task<int> CreateAsync(CategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _categories.AddAsync(entity);
            var rows = await _categories.SaveChangesAsync(CancellationToken.None);
            return rows; // instead of: return entity.Id;
        }

        public async Task<bool> UpdateAsync(CategoryDto dto)
        {
            var e = await _categories.GetByIdAsync(dto.Id);
            if (e == null) return false;
            e.Name = dto.Name;
            _categories.Update(e);
            await _categories.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var e = await _categories.GetByIdAsync(id);
            if (e == null) return false;
            e.IsDeleted = true;
            _categories.Update(e);
            await _categories.SaveChangesAsync();
            return true;
        }
    }
}
