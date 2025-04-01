using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Wallet;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : BaseApiController
{
    private readonly IWalletService _walletService;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public WalletController(
        IWalletService walletService,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _walletService = walletService;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(WalletDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWallet()
    {
        var userId = int.Parse(_currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated"));
        var result = await _walletService.GetWalletAsync(userId);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    [HttpPost("deposit")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
    {
        var userId = int.Parse(_currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated"));
        var result = await _walletService.DepositAsync(userId, request);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        await _notificationService.SendNotificationAsync(
            userId.ToString(),
            $"تم إيداع {request.Amount:C} في محفظتك بنجاح",
            "/wallet");

        return Ok(result);
    }

    [HttpPost("withdraw")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
    {
        var userId = int.Parse(_currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated"));
        var result = await _walletService.WithdrawAsync(userId, request);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        await _notificationService.SendNotificationAsync(
            userId.ToString(),
            $"تم سحب {request.Amount:C} من محفظتك بنجاح",
            "/wallet");

        return Ok(result);
    }

    [HttpPost("transfer")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        var userId = int.Parse(_currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated"));
        var result = await _walletService.TransferAsync(userId, request);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        await _notificationService.SendNotificationAsync(
            userId.ToString(),
            $"تم تحويل {request.Amount:C} إلى المستخدم {request.RecipientId} بنجاح",
            "/wallet");

        return Ok(result);
    }

    [HttpGet("transactions")]
    [ProducesResponseType(typeof(PagedList<TransactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions([FromQuery] TransactionParams parameters)
    {
        var userId = int.Parse(_currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated"));
        var result = await _walletService.GetTransactionsAsync(userId, parameters);
        return Ok(result);
    }
}