using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(BountyHuntersDbContext db) : base(db) { }

    public Task<Comment?> GetByIdWithIncludesAsync(int id)
        => Db<Comment>()
            .Include(c => c.User)
            .Include(c => c.Mission)
            .Include(c => c.Likes)
            .FirstOrDefaultAsync(c => c.Id == id);
}
