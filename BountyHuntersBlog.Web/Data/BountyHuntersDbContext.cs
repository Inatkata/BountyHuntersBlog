using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext : IdentityDbContext<ApplicationUser>
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options)
        {
        }

        public DbSet<MissionPost> MissionPosts { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<Hunter> Hunters { get; set; }
        public DbSet<MissionComment> MissionComments { get; set; }
        public DbSet<MissionLike> MissionLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            // Едно-към-много: MissionPost -> Comments
            builder.Entity<MissionComment>()
                .HasOne(mc => mc.MissionPost)
                .WithMany(mp => mp.Comments)
                .HasForeignKey(mc => mc.MissionPostId);

            // Едно-към-много: Hunter -> Comments
            builder.Entity<MissionComment>()
                .HasOne(mc => mc.Hunter)
                .WithMany()
                .HasForeignKey(mc => mc.HunterId);

            // Едно-към-много: MissionPost -> Likes
            builder.Entity<MissionLike>()
                .HasOne(ml => ml.MissionPost)
                .WithMany(mp => mp.Likes)
                .HasForeignKey(ml => ml.MissionPostId);

            // Едно-към-много: Hunter -> Likes
            builder.Entity<MissionLike>()
                .HasOne(ml => ml.Hunter)
                .WithMany()
                .HasForeignKey(ml => ml.HunterId);

            // Едно-към-много: ApplicationUser -> MissionPost
            builder.Entity<MissionPost>()
                .HasOne(mp => mp.Author)
                .WithMany()
                .HasForeignKey(mp => mp.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
