namespace MzadPalestine.Core.Settings;

public class CacheSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string InstanceName { get; set; } = "MzadPalestine_";
    public int DefaultExpirationMinutes { get; set; } = 60;
}
