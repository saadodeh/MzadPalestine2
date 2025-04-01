using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Transactions;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ICurrentUserService _currentUserService;

    public TransactionController(IPaymentService paymentService, ICurrentUserService currentUserService)
    {
        _paymentService = paymentService;
        _currentUserService = currentUserService;
    }

    [HttpPost("pay")]
    public async Task<IActionResult> ProcessPayment([FromBody] TransactionPaymentDto paymentDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _paymentService.ProcessPaymentAsync(userId, paymentDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpPut("refund/{transactionId}")]
    public async Task<IActionResult> RefundPayment(string transactionId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _paymentService.RefundPaymentAsync(userId, transactionId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Refund processed successfully" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserTransactions(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId)
            return Forbid();

        var result = await _paymentService.GetUserTransactionsAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetTransaction(string transactionId)
    {
        var result = await _paymentService.GetTransactionByIdAsync(transactionId);
        
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }
}
