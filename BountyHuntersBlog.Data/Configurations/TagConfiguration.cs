using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(t => t.MissionTags)
                .WithOne(mt => mt.Tag)
                .HasForeignKey(mt => mt.TagId);
        }
    }
}