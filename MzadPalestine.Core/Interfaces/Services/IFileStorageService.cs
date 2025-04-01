namespace MzadPalestine.Core.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    Task<bool> DeleteFileAsync(string fileUrl);
    Task<string> GetFileUrlAsync(string fileName);
    bool IsValidFileExtension(string fileName);
    bool IsValidFileSize(long fileSize);
    Task<string> SaveFileAsync(Stream fileStream, string fileName);
}
