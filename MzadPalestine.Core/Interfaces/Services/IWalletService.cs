using MzadPalestine.Core.DTOs.Wallet;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IWalletService
{
    Task<Result<WalletDto>> GetWalletAsync(int userId);
    Task<Result<bool>> DepositAsync(int userId , DepositRequest request);
    Task<Result<bool>> WithdrawAsync(int userId , WithdrawRequest request);
    Task<Result<bool>> TransferAsync(int userId , TransferRequest request);
    Task<PagedList<TransactionDto>> GetTransactionsAsync(int userId , TransactionParams parameters);
}