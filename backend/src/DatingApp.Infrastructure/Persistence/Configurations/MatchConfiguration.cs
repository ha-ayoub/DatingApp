using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasOne(m => m.User1).WithMany()
            .HasForeignKey(m => m.User1Id).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.User2).WithMany()
            .HasForeignKey(m => m.User2Id).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(m => m.Messages).WithOne(msg => msg.Match)
            .HasForeignKey(msg => msg.MatchId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(m => new { m.User1Id, m.User2Id }).IsUnique();
        builder.HasIndex(m => m.IsActive);
    }
}
