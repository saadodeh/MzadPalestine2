using Microsoft.AspNetCore.Http;
using MzadPalestine.Core.Interfaces.Services;

namespace MzadPalestine.Infrastructure.Services.FileStorage;

public class PhotoService : IPhotoService
{
    private readonly IFileStorageService _fileStorageService;

    public PhotoService(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<string> UploadPhotoAsync(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        return await _fileStorageService.UploadFileAsync(stream, file.FileName, file.ContentType);
    }

    public async Task<bool> DeletePhotoAsync(string photoUrl)
    {
        return await _fileStorageService.DeleteFileAsync(photoUrl);
    }

    public async Task<string> UpdatePhotoAsync(string oldPhotoUrl, IFormFile newFile)
    {
        await DeletePhotoAsync(oldPhotoUrl);
        return await UploadPhotoAsync(newFile);
    }
}