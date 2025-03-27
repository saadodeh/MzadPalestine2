using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Wallet;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.API.Controllers;

[Authorize]
public class WalletController : BaseApiController
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;

    public WalletController(
        IWalletRepository walletRepository,
        ICurrentUserService currentUserService,
        IPaymentService paymentService,
        INotificationService notificationService)
    {
        _walletRepository = walletRepository;
        _currentUserService = currentUserService;
        _paymentService = paymentService;
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<ActionResult<WalletDto>> GetWallet()
    {
        var wallet = await _walletRepository.GetUserWalletAsync(_currentUserService.UserId!);

        if (wallet == null)
        {
            return NotFound();
        }

        return new WalletDto
        {
            Id = wallet.Id,
            Balance = wallet.Balance,
            IsActive = wallet.IsActive,
            RecentTransactions = wallet.Transactions
                .OrderByDescending(t => t.CreatedAt)
                .Take(10)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type,
                    Description = t.Description,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                }).ToList()
        };
    }

    [HttpGet("transactions")]
    public async Task<ActionResult<PagedList<TransactionDto>>> GetTransactions([FromQuery] TransactionParams parameters)
    {
        var transactions = await _walletRepository.GetUserTransactionsAsync(
            _currentUserService.UserId!,
            parameters.StartDate,
            parameters.EndDate,
            parameters.Type);

        var pagedTransactions = new PagedList<TransactionDto>(
            transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            }).ToList(),
            transactions.Count(),
            parameters.PageNumber,
            parameters.PageSize);

        return pagedTransactions;
    }

    [HttpPost("deposit")]
    public async Task<ActionResult<TransactionDto>> Deposit(DepositRequest request)
    {
        var paymentResult = await _paymentService.ProcessPaymentAsync(
            request.Amount,
            "USD",
            request.PaymentMethodId);

        if (!paymentResult.Success)
        {
            return BadRequest(paymentResult.ErrorMessage);
        }

        var transaction = await _walletRepository.AddTransactionAsync(
            _currentUserService.UserId!,
            request.Amount,
            TransactionType.Credit,
            $"إيداع عن طريق {request.PaymentMethodId}");

        await _notificationService.SendNotificationAsync(
            _currentUserService.UserId!,
            $"تم إيداع {request.Amount:C} في محفظتك بنجاح",
            "/wallet");

        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            Status = transaction.Status,
            CreatedAt = transaction.CreatedAt
        };
    }

    [HttpPost("withdraw")]
    public async Task<ActionResult<TransactionDto>> Withdraw(WithdrawRequest request)
    {
        var hasBalance = await _walletRepository.HasSufficientBalanceAsync(
            _currentUserService.UserId!,
            request.Amount);

        if (!hasBalance)
        {
            return BadRequest("رصيد غير كافي");
        }

        var transaction = await _walletRepository.AddTransactionAsync(
            _currentUserService.UserId!,
            request.Amount,
            TransactionType.Debit,
            $"سحب إلى {request.BankAccount}");

        await _notificationService.SendNotificationAsync(
            _currentUserService.UserId!,
            $"تم سحب {request.Amount:C} من محفظتك بنجاح",
            "/wallet");

        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            Status = transaction.Status,
            CreatedAt = transaction.CreatedAt
        };
    }

    [HttpPost("transfer")]
    public async Task<ActionResult<TransactionDto>> Transfer(TransferRequest request)
    {
        if (request.RecipientId == _currentUserService.UserId)
        {
            return BadRequest("لا يمكن التحويل لنفس الحساب");
        }

        var hasBalance = await _walletRepository.HasSufficientBalanceAsync(
            _currentUserService.UserId!,
            request.Amount);

        if (!hasBalance)
        {
            return BadRequest("رصيد غير كافي");
        }

        // Debit from sender
        var senderTransaction = await _walletRepository.AddTransactionAsync(
            _currentUserService.UserId!,
            request.Amount,
            TransactionType.Debit,
            $"تحويل إلى مستخدم #{request.RecipientId}");

        // Credit to recipient
        var recipientTransaction = await _walletRepository.AddTransactionAsync(
            request.RecipientId,
            request.Amount,
            TransactionType.Credit,
            $"تحويل من مستخدم #{_currentUserService.UserId}");

        // Notify both parties
        await _notificationService.SendNotificationAsync(
            _currentUserService.UserId!,
            $"تم تحويل {request.Amount:C} بنجاح",
            "/wallet");

        await _notificationService.SendNotificationAsync(
            request.RecipientId,
            $"تم استلام {request.Amount:C} من تحويل",
            "/wallet");

        return new TransactionDto
        {
            Id = senderTransaction.Id,
            Amount = senderTransaction.Amount,
            Type = senderTransaction.Type,
            Description = senderTransaction.Description,
            Status = senderTransaction.Status,
            CreatedAt = senderTransaction.CreatedAt
        };
    }
}
