using DatingApp.Domain.Common;
using DatingApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<Swipe> Swipes => Set<Swipe>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<UserBlock> UserBlocks => Set<UserBlock>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await base.SaveChangesAsync(ct);
        await DispatchDomainEventsAsync(ct);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity).ToList();
        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());
        foreach (var ev in events)
            await mediator.Publish(ev, ct);
    }
}
