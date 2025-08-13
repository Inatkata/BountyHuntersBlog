// BountyHuntersBlog.Data/Configurations/CommentConfiguration.cs
using BountyHuntersBlog.Data.Constants;
using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BountyHuntersBlog.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(ModelConstants.Comment.ContentMaxLength);

            // user
            builder.Property(c => c.UserId)
                .HasColumnName("UserId"); 

            builder.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // mission
            builder.HasOne(c => c.Mission)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // индекси
            builder.HasIndex(c => new { c.MissionId, c.CreatedOn }).HasDatabaseName("IX_Comments_MissionId_CreatedOn");
        }
    }
}