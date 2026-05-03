using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Content).HasMaxLength(1000).IsRequired();
        builder.HasOne(m => m.Sender).WithMany()
            .HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(m => m.MatchId);
        builder.HasIndex(m => m.CreatedAt);
    }
}
