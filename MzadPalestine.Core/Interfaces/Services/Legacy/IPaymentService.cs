using System.Threading.Tasks;

namespace MzadPalestine.Core.Interfaces.Services.Legacy;

public interface IPaymentService
{
    Task<string> InitiatePaymentAsync(int amount, string currency);
    Task<bool> ProcessPaymentAsync(string paymentMethodId, string currency);
}