using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Payments;
using MzadPalestine.Core.DTOs.Transactions;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public PaymentController(
        IPaymentService paymentService,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _paymentService = paymentService;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(PaymentResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessPayment([FromBody] MzadPalestine.Core.DTOs.Payments.ProcessPaymentDto paymentDto)
    {
        var userId = _currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated");
        var transactionPaymentDto = new TransactionPaymentDto
        {
            PaymentMethodId = paymentDto.PaymentMethodId,
            Amount = paymentDto.Amount,
            Currency = paymentDto.Currency
        };
        var result = await _paymentService.ProcessPaymentAsync(userId, transactionPaymentDto);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        await _notificationService.SendNotificationAsync(
            userId,
            $"Payment processed successfully for {paymentDto.Amount:C}",
            "/payments");

        return Ok(result.Data);
    }

    [HttpPost("refund/{transactionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefundPayment(string transactionId)
    {
        var userId = _currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated");
        var result = await _paymentService.RefundPaymentAsync(userId, transactionId);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        await _notificationService.SendNotificationAsync(
            userId,
            "Refund processed successfully",
            "/payments");

        return Ok(new { message = "Refund processed successfully" });
    }

    [HttpGet("transactions")]
    [ProducesResponseType(typeof(PagedList<TransactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions([FromQuery] PaginationParams parameters)
    {
        var userId = _currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated");
        var result = await _paymentService.GetUserTransactionsAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("transaction/{transactionId}")]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransaction(string transactionId)
    {
        var result = await _paymentService.GetTransactionByIdAsync(transactionId);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }
}