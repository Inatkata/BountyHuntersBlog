using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data.Configurations
{
    public class MissionConfiguration : IEntityTypeConfiguration<Mission>
    {
        public void Configure(EntityTypeBuilder<Mission> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Description)
                .IsRequired();

            builder.Property(m => m.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.Author)
                .WithMany(u => u.Missions)
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Category)
                .WithMany(c => c.Missions)
                .HasForeignKey(m => m.CategoryId);

            builder.HasMany(m => m.MissionTags)
                .WithOne(mt => mt.Mission)
                .HasForeignKey(mt => mt.MissionId);
        }
    }
}