using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TagRepository : Repository<Tag>, ITagRepository
{
    private readonly BountyHuntersDbContext _ctx;
    public TagRepository(BountyHuntersDbContext context)
        : base(context)
    {
        _ctx = context; 
    }

    public async Task<IReadOnlyList<Tag>> AllAsync()
        => await _ctx.Tags.AsNoTracking().ToListAsync();

    public async Task<Tag?> GetByIdAsync(int id)
        => await _ctx.Tags.FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(Tag entity) => await _ctx.Tags.AddAsync(entity);
    public void Update(Tag entity) => _ctx.Tags.Update(entity);
    public void Delete(Tag entity) => _ctx.Tags.Remove(entity);
    public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

    public Task<bool> ExistsAsync(int id)           // <— ИМПЛЕМЕНТАЦИЯ
        => _ctx.Tags.AnyAsync(t => t.Id == id);
}