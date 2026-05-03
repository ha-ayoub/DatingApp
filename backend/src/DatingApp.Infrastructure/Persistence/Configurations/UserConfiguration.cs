using DatingApp.Domain.Entities;
using DatingApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Bio).HasMaxLength(500);
        builder.Property(u => u.City).HasMaxLength(100);
        builder.Property(u => u.Country).HasMaxLength(100);
        builder.Property(u => u.RefreshToken).HasMaxLength(512);

        // ValueConverter + ValueComparer pour GenderPreferences
        var comparer = new ValueComparer<List<Gender>>(
            (a, b) => a != null && b != null && a.SequenceEqual(b),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        builder.Property(u => u.GenderPreferences)
            .HasConversion(
                v => string.Join(',', v.Select(g => (int)g)),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(g => (Gender)int.Parse(g)).ToList()
            )
            .Metadata.SetValueComparer(comparer);

        builder.HasMany(u => u.Photos).WithOne(p => p.User)
            .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(u => new { u.Latitude, u.Longitude });
        builder.HasIndex(u => u.DateOfBirth);
        builder.HasIndex(u => u.Gender);
        builder.HasIndex(u => u.IsActive);
    }
}