using DatingApp.Domain.Entities;
using DatingApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Repositories;

public class PhotoRepository(AppDbContext db) : IPhotoRepository
{
    public Task AddAsync(Photo photo, CancellationToken ct = default)
        => db.Photos.AddAsync(photo, ct).AsTask();

    public Task<Photo?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Photos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public void Remove(Photo photo) => db.Photos.Remove(photo);
}