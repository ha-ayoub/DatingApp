using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DatingApp.Infrastructure.Services.External;

public class CloudinaryService(IConfiguration config) : ICloudinaryService
{
    private readonly Cloudinary _cloudinary = new(new Account(
        config["Cloudinary:CloudName"],
        config["Cloudinary:ApiKey"],
        config["Cloudinary:ApiSecret"]));

    public async Task<(string PublicId, string Url)> UploadPhotoAsync(Stream stream, string fileName, CancellationToken ct = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = "dating-app/photos",
            Transformation = new Transformation().Width(800).Height(800).Crop("fill").Quality("auto")
        };
        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.Error is not null) throw new Exception(result.Error.Message);
        return (result.PublicId, result.SecureUrl.ToString());
    }

    public async Task DeletePhotoAsync(string publicId, CancellationToken ct = default)
        => await _cloudinary.DestroyAsync(new DeletionParams(publicId));
}
