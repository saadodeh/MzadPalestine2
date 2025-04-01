using Microsoft.AspNetCore.Http;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IPhotoService
{
    Task<string> UploadPhotoAsync(IFormFile file);
    Task<bool> DeletePhotoAsync(string photoUrl);
    Task<string> UpdatePhotoAsync(string oldPhotoUrl, IFormFile newFile);
}