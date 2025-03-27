using MzadPalestine.Core.DTOs.Users;

namespace MzadPalestine.Core.DTOs.Subscriptions;

public class SubscriptionDto
{
    public int Id { get; set; }
    public UserDto User { get; set; } = null!;
    public string Plan { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? RenewalDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? PaymentMethodId { get; set; }
}
