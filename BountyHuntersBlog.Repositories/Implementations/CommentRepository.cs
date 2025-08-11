using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(BountyHuntersDbContext ctx) : base(ctx) { }

    public Task<Comment?> GetByIdWithIncludesAsync(int id)
        => _db.Set<Comment>()
            .Include(c => c.Mission)
            .Include(c => c.User)
            .Include(c => c.Likes)
            .FirstOrDefaultAsync(c => c.Id == id);
}