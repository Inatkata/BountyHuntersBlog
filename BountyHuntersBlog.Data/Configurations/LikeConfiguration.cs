// BountyHuntersBlog.Data/Configurations/LikeConfiguration.cs
using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> b)
    {
        b.Property(l => l.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

        b.HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(l => l.Mission)
            .WithMany(m => m.Likes)
            .HasForeignKey(l => l.MissionId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(l => l.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(l => l.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        // XOR: точно едно от MissionId / CommentId да е зададено
        b.HasCheckConstraint(
            "CK_Like_Target_XOR",
            "((MissionId IS NULL AND CommentId IS NOT NULL) OR (MissionId IS NOT NULL AND CommentId IS NULL))"
        );

        // Уникален like от потребител към мисия (ако MissionId е зададен)
        b.HasIndex(l => new { l.UserId, l.MissionId })
            .IsUnique()
            .HasFilter("[MissionId] IS NOT NULL");

        // Уникален like от потребител към коментар (ако CommentId е зададен)
        b.HasIndex(l => new { l.UserId, l.CommentId })
            .IsUnique()
            .HasFilter("[CommentId] IS NOT NULL");

        b.HasIndex(l => l.CreatedOn);
    }
}