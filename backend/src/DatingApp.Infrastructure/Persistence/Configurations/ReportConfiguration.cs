using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Description).HasMaxLength(1000);
        builder.HasOne(r => r.Reporter).WithMany(u => u.Reports)
            .HasForeignKey(r => r.ReporterId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(r => r.ReportedUser).WithMany()
            .HasForeignKey(r => r.ReportedUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
