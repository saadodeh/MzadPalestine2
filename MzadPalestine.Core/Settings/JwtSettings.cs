namespace MzadPalestine.Core.Settings;

public class JwtSettings
{
  
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
    public int AccessTokenExpirationMinutes { get; set; } = 60; 

}
