using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Data.Constants;

public class MissionConfiguration : IEntityTypeConfiguration<Mission>
{
    public void Configure(EntityTypeBuilder<Mission> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(ModelConstants.User.DisplayNameMaxLength);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(ModelConstants.User.DisplayNameMaxLength);

        builder.Property(m => m.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

        // По избор, ако има такива полета в ентитито:
        // builder.Property(m => m.IsCompleted).HasDefaultValue(false);
        // builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasOne(m => m.User)
            .WithMany(u => u.Missions)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Category)
            .WithMany(c => c.Missions)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(m => !m.IsDeleted);
        // Индекси за търсене / списъци
        builder.HasIndex(m => m.Title);
        builder.HasIndex(m => new { m.CategoryId, m.CreatedOn });
    }
}