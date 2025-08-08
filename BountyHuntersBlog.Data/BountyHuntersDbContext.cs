
using Microsoft.EntityFrameworkCore;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mission> Missions { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<MissionTag> MissionTags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(typeof(BountyHuntersDbContext).Assembly);
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new MissionConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new LikeConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new MissionTagConfiguration());
        }
    }
}