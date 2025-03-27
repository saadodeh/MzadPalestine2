namespace MzadPalestine.Core.Settings;

public class FileStorageSettings
{
    public string BasePath { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB default
    public string[] AllowedExtensions { get; set; } = new[] { ".jpg", ".jpeg", ".png", ".gif" };
    public bool UseCloudStorage { get; set; }
    public string CloudStorageProvider { get; set; } = "Local"; // Local, Azure, AWS, etc.
    public string CloudStorageConnectionString { get; set; } = string.Empty;
    public string CloudStorageContainerName { get; set; } = string.Empty;
}
