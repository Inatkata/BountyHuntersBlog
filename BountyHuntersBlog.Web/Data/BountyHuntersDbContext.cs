using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext : IdentityDbContext<Hunter, IdentityRole, string>
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options)
        {
        }

        public DbSet<MissionPost> MissionPosts { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<MissionLike> MissionLikes { get; set; }
        public DbSet<MissionComment> MissionComments { get; set; }
        public DbSet<Hunter> Hunters { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ясно казваме, че ще използваме Hunter като юзър
            builder.Entity<Hunter>().ToTable("AspNetUsers");

            builder.Entity<MissionPost>()
                .HasOne(mp => mp.Author)
                .WithMany(h => h.MissionPosts)
                .HasForeignKey(mp => mp.AuthorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<MissionPost>()
                .HasMany(m => m.Factions)
                .WithMany(f => f.MissionPosts);

            builder.Entity<MissionLike>()
                .HasKey(l => new { l.MissionPostId, l.HunterId });

            builder.Entity<MissionComment>()
                .HasOne(c => c.MissionPost)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MissionPostId);
        }

    }
}