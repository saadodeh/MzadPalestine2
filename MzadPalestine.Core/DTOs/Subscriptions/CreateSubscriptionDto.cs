using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Subscriptions;

public class CreateSubscriptionDto
{
    [Required]
    public string Plan { get; set; } = string.Empty;

    [Required]
    public string PaymentMethodId { get; set; } = string.Empty;
}
