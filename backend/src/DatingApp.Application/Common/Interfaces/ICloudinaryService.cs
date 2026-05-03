namespace DatingApp.Application.Common.Interfaces;

public interface ICloudinaryService
{
    Task<(string PublicId, string Url)> UploadPhotoAsync(Stream stream, string fileName, CancellationToken ct = default);
    Task DeletePhotoAsync(string publicId, CancellationToken ct = default);
}
