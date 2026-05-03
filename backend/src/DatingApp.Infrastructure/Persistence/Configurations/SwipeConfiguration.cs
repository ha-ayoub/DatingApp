using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Configurations;

public class SwipeConfiguration : IEntityTypeConfiguration<Swipe>
{
    public void Configure(EntityTypeBuilder<Swipe> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Swiper).WithMany(u => u.SentSwipes)
            .HasForeignKey(s => s.SwiperId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.Swiped).WithMany(u => u.ReceivedSwipes)
            .HasForeignKey(s => s.SwipedId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(s => new { s.SwiperId, s.SwipedId }).IsUnique();
    }
}
