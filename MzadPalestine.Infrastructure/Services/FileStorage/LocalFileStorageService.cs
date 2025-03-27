using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MzadPalestine.Core.Constants;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Settings;

namespace MzadPalestine.Infrastructure.Services.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly FileStorageSettings _settings;
    private readonly string _uploadPath;

    public LocalFileStorageService(
        IWebHostEnvironment environment,
        IOptions<FileStorageSettings> settings)
    {
        _environment = environment;
        _settings = settings.Value;
        _uploadPath = Path.Combine(_environment.WebRootPath, AppConstants.FileUpload.UploadDirectory);
        
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        if (!IsValidFileExtension(fileName))
        {
            throw new InvalidOperationException("Invalid file extension");
        }

        if (!IsValidFileSize(fileStream.Length))
        {
            throw new InvalidOperationException("File size exceeds maximum limit");
        }

        var uniqueFileName = GetUniqueFileName(fileName);
        var filePath = Path.Combine(_uploadPath, uniqueFileName);

        using (var fileWriter = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fileWriter);
        }

        return $"/{AppConstants.FileUpload.UploadDirectory}/{uniqueFileName}";
    }

    public Task<bool> DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
        {
            return Task.FromResult(false);
        }

        var fileName = Path.GetFileName(fileUrl);
        var filePath = Path.Combine(_uploadPath, fileName);

        if (!File.Exists(filePath))
        {
            return Task.FromResult(false);
        }

        try
        {
            File.Delete(filePath);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<string> GetFileUrlAsync(string fileName)
    {
        return Task.FromResult($"/{AppConstants.FileUpload.UploadDirectory}/{fileName}");
    }

    public bool IsValidFileExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        return !string.IsNullOrEmpty(extension) && 
               AppConstants.FileUpload.AllowedImageExtensions.Split(',').Contains(extension);
    }

    public bool IsValidFileSize(long fileSize)
    {
        return fileSize <= AppConstants.FileUpload.MaxFileSize;
    }

    private string GetUniqueFileName(string fileName)
    {
        var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        return uniqueName;
    }
}
