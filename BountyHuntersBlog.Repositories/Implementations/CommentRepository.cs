using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class CommentRepository: Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BountyHuntersDbContext context)
            : base(context) { }
        public async Task<IEnumerable<Comment>> GetCommentsByMissionIdAsync(int missionId)
            => await Context.Comments
                .Where(c => c.MissionId == missionId)
                .ToListAsync();
        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string authorId)
            => await Context.Comments
                .Where(c => c.AuthorId == authorId)
                .ToListAsync();

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
            => await DbSet.FindAsync(commentId);
        public async Task<bool> ExistsAsync(int commentId)
            => await DbSet.AnyAsync(c => c.Id == commentId);
        public async Task RemoveByIdAsync(int commentId)
        {
            var comment = await GetCommentByIdAsync(commentId);
            if (comment != null)
            {
                Delete(comment);
                await SaveChangesAsync();
            }
        }
        public async Task<int> CountCommentsByMissionIdAsync(int missionId)
            => await DbSet.CountAsync(c => c.MissionId == missionId);


        public async Task<int> CountCommentsByUserIdAsync(string userId)
            => await Context.Comments.CountAsync(c=>c.AuthorId == userId);
    }
}
