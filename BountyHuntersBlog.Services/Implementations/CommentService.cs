using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;

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
        // if your base repo has AllAsync(), use it + paging; otherwise add one
        var all = await _repo.AllAsync();
        var pageItems = all
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        return _mapper.Map<IEnumerable<CommentDto>>(pageItems);
    }

    public async Task<CommentDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetCommentByIdAsync(id);
        return entity is null ? null : _mapper.Map<CommentDto>(entity);
    }

    public async Task CreateAsync(CommentDto dto)
    {
        var entity = _mapper.Map<Comment>(dto);
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, CommentDto dto)
    {
        var entity = await _repo.GetCommentByIdAsync(id)
                     ?? throw new KeyNotFoundException($"Comment {id} not found");
        _mapper.Map(dto, entity);
        _repo.Update(entity);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _repo.ExistsAsync(id);
        if (!exists) throw new KeyNotFoundException($"Comment {id} not found");
        await _repo.RemoveByIdAsync(id);
    }

    public Task<bool> ExistsAsync(int id) => _repo.ExistsAsync(id);

    public async Task<IEnumerable<CommentDto>> GetCommentsByMissionIdAsync(int missionId)
    {
        var list = await _repo.GetCommentsByMissionIdAsync(missionId);
        return _mapper.Map<IEnumerable<CommentDto>>(list);
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(string userId)
    {
        var list = await _repo.GetCommentsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<CommentDto>>(list);
    }
}
