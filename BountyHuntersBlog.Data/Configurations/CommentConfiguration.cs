// BountyHuntersBlog.Data/Configurations/CommentConfiguration.cs
using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Constants;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> b)
    {
        b.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(ModelConstants.User.DisplayNameMaxLength);

        b.Property(c => c.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

        b.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(c => c.Mission)
            .WithMany(m => m.Comments)
            .HasForeignKey(c => c.MissionId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(c => new { c.MissionId, c.CreatedOn });
    }
}