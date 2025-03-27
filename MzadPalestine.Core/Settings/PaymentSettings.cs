namespace MzadPalestine.Core.Settings;

public class PaymentSettings
{
    public string[] AllowedCurrencies { get; set; } = new[] { "USD", "ILS", "JOD" };
    public string PayPalClientId { get; set; } = string.Empty;
    public string PayPalClientSecret { get; set; } = string.Empty;
    public string PayPalEnvironment { get; set; } = "sandbox";
    public decimal MinimumPaymentAmount { get; set; } = 1.00m;
    public decimal MaximumPaymentAmount { get; set; } = 10000.00m;
    public int PaymentExpirationMinutes { get; set; } = 30;
}
