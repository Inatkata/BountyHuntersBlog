using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<MissionPost> MissionPosts { get; set; }
        public DbSet<MissionLike> MissionLikes { get; set; }
        public DbSet<MissionComment> MissionComments { get; set; }
        public DbSet<Faction> Factions { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<MissionPost>()
                .HasOne(mp => mp.PostedByUser)
                .WithMany(u => u.PostedMissions)
                .HasForeignKey(mp => mp.PostedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MissionPost>()
                .HasOne(mp => mp.TakenByApplicationUser)
                .WithMany(u => u.TakenMissions)
                .HasForeignKey(mp => mp.TakenByApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<MissionLike>()
                .HasKey(l => new { l.MissionPostId, l.ApplicationUserId });

            builder.Entity<MissionComment>()
                .HasOne(c => c.MissionPost)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MissionPostId);
        }

    }
}