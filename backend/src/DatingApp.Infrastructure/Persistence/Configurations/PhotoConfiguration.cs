using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.PublicId).HasMaxLength(256).IsRequired();
        builder.Property(p => p.Url).HasMaxLength(512).IsRequired();
        builder.HasIndex(p => p.UserId);
    }
}
