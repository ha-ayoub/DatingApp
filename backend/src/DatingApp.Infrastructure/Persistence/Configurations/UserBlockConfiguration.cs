using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Configurations;

public class UserBlockConfiguration : IEntityTypeConfiguration<UserBlock>
{
    public void Configure(EntityTypeBuilder<UserBlock> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasOne(b => b.Blocker).WithMany(u => u.BlockedUsers)
            .HasForeignKey(b => b.BlockerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(b => b.Blocked).WithMany()
            .HasForeignKey(b => b.BlockedId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(b => new { b.BlockerId, b.BlockedId }).IsUnique();
    }
}
