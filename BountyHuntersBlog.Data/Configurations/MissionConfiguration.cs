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
            .HasMaxLength(ModelConstants.MissionTitleMaxLength);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(ModelConstants.MissionDescriptionMaxLength);

        builder.Property(m => m.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(m => m.Author)
            .WithMany(u => u.Missions)
            .HasForeignKey(m => m.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Category)
            .WithMany(c => c.Missions)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}