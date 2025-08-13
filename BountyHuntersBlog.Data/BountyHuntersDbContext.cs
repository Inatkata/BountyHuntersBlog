using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext : IdentityDbContext<ApplicationUser>
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options) { }

        public DbSet<Mission> Missions => Set<Mission>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<MissionTag> MissionTags => Set<MissionTag>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(BountyHuntersDbContext).Assembly);

            builder.Entity<Mission>().HasQueryFilter(m => !m.IsDeleted);
            builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Tag>().HasQueryFilter(t => !t.IsDeleted);
            builder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);

            // избегни EF warning за required навигации + филтри
            builder.Entity<MissionTag>()
                .HasQueryFilter(mt => !mt.Mission.IsDeleted && !mt.Tag.IsDeleted);


        }
    }
}