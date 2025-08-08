using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired();

            builder.Property(c => c.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(c => c.Mission)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Likes)
                .WithOne(l => l.Comment)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}