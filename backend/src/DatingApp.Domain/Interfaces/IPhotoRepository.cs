using DatingApp.Domain.Entities;

namespace DatingApp.Domain.Interfaces;

public interface IPhotoRepository
{
    Task AddAsync(Photo photo, CancellationToken ct = default);
    Task<Photo?> GetByIdAsync(Guid id, CancellationToken ct = default);
    void Remove(Photo photo);
}