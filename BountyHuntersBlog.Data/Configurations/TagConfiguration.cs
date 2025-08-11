// BountyHuntersBlog.Data/Configurations/TagConfiguration.cs
using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Constants;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> b)
    {
        b.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(ModelConstants.User.DisplayNameMaxLength);

        b.Property(t => t.IsDeleted).HasDefaultValue(false);
        b.Property(t => t.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

        // Уникален индекс
        b.HasIndex(t => t.Name).IsUnique();

        // Връзка MissionTag
        b.HasMany(t => t.MissionTags)
            .WithOne(mt => mt.Tag)
            .HasForeignKey(mt => mt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}