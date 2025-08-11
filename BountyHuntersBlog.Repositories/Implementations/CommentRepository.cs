// Implementations/CommentRepository.cs
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BountyHuntersDbContext ctx) : base(ctx) { }

        public Task<Comment?> GetByIdWithIncludesAsync(int id) =>
            _dbSet
                .Include(c => c.User)
                .Include(c => c.Mission)
                .FirstOrDefaultAsync(c => c.Id == id);
    }
}