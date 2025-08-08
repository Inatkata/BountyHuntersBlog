using BountyHuntersBlog.Data;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Repositories.Base
{
    public class UnitOfWork
    {
        private readonly BountyHuntersDbContext _context;
        public UnitOfWork(BountyHuntersDbContext context)
            => _context = context;

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();
    }
}