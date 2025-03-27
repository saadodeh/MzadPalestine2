using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Payments;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.API.Controllers;

[Authorize]
public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;

    public PaymentsController(
        IPaymentService paymentService,
        IAuctionRepository auctionRepository,
        IWalletRepository walletRepository,
        ICurrentUserService currentUserService,
        INotificationService notificationService,
        IEmailService emailService)
    {
        _paymentService = paymentService;
        _auctionRepository = auctionRepository;
        _walletRepository = walletRepository;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
        _emailService = emailService;
    }

    [HttpPost("auction/{auctionId}")]
    public async Task<ActionResult<PaymentResultDto>> ProcessAuctionPayment(int auctionId, [FromBody] ProcessPaymentRequest request)
    {
        var auction = await _auctionRepository.GetAuctionDetailsAsync(auctionId);

        if (auction == null)
        {
            return NotFound("المزاد غير موجود");
        }

        if (auction.Status != AuctionStatus.Completed)
        {
            return BadRequest("المزاد لم ينتهي بعد");
        }

        if (auction.WinnerId != _currentUserService.UserId)
        {
            return Forbid();
        }

        if (auction.PaymentStatus == PaymentStatus.Paid)
        {
            return BadRequest("تم الدفع مسبقاً");
        }

        // Check if using wallet balance
        if (request.UseWalletBalance)
        {
            var hasBalance = await _walletRepository.HasSufficientBalanceAsync(
                _currentUserService.UserId!,
                auction.CurrentPrice);

            if (!hasBalance)
            {
                return BadRequest("رصيد المحفظة غير كافي");
            }

            // Process wallet payment
            await _walletRepository.AddTransactionAsync(
                _currentUserService.UserId!,
                auction.CurrentPrice,
                TransactionType.Debit,
                $"دفع المزاد #{auctionId}");

            // Credit seller's wallet
            await _walletRepository.AddTransactionAsync(
                auction.Listing!.SellerId,
                auction.CurrentPrice,
                TransactionType.Credit,
                $"عائد المزاد #{auctionId}");

            auction.PaymentStatus = PaymentStatus.Paid;
            await _auctionRepository.UpdateAsync(auction);

            // Send notifications
            await SendPaymentNotifications(auction);

            return new PaymentResultDto
            {
                Success = true,
                TransactionId = Guid.NewGuid().ToString(),
                Amount = auction.CurrentPrice,
                PaymentMethod = "wallet"
            };
        }

        // Process external payment
        var paymentResult = await _paymentService.ProcessPaymentAsync(
            auction.CurrentPrice,
            "USD",
            request.PaymentMethodId);

        if (!paymentResult.Success)
        {
            return BadRequest(paymentResult.ErrorMessage);
        }

        auction.PaymentStatus = PaymentStatus.Paid;
        await _auctionRepository.UpdateAsync(auction);

        // Send notifications
        await SendPaymentNotifications(auction);

        return new PaymentResultDto
        {
            Success = true,
            TransactionId = paymentResult.TransactionId!,
            Amount = auction.CurrentPrice,
            PaymentMethod = request.PaymentMethodId
        };
    }

    [HttpPost("refund/{auctionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RefundResultDto>> ProcessRefund(int auctionId)
    {
        var auction = await _auctionRepository.GetAuctionDetailsAsync(auctionId);

        if (auction == null)
        {
            return NotFound("المزاد غير موجود");
        }

        if (auction.PaymentStatus != PaymentStatus.Paid)
        {
            return BadRequest("لم يتم الدفع بعد");
        }

        // Process refund
        var refundResult = await _paymentService.RefundPaymentAsync(auction.PaymentTransactionId!);

        if (!refundResult.Success)
        {
            return BadRequest(refundResult.ErrorMessage);
        }

        auction.PaymentStatus = PaymentStatus.Refunded;
        await _auctionRepository.UpdateAsync(auction);

        // Notify users
        await _notificationService.SendNotificationAsync(
            auction.WinnerId!,
            $"تم استرداد مبلغ {auction.CurrentPrice:C} من المزاد #{auctionId}",
            $"/auctions/{auctionId}");

        await _notificationService.SendNotificationAsync(
            auction.Listing!.SellerId,
            $"تم استرداد مبلغ {auction.CurrentPrice:C} للمزاد #{auctionId}",
            $"/auctions/{auctionId}");

        return new RefundResultDto
        {
            Success = true,
            RefundTransactionId = refundResult.TransactionId!,
            Amount = auction.CurrentPrice
        };
    }

    private async Task SendPaymentNotifications(Auction auction)
    {
        // Notify winner
        await _notificationService.SendNotificationAsync(
            auction.WinnerId!,
            $"تم تأكيد الدفع للمزاد #{auction.Id} بمبلغ {auction.CurrentPrice:C}",
            $"/auctions/{auction.Id}");

        // Notify seller
        await _notificationService.SendNotificationAsync(
            auction.Listing!.SellerId,
            $"تم استلام الدفع للمزاد #{auction.Id} بمبلغ {auction.CurrentPrice:C}",
            $"/auctions/{auction.Id}");

        // Send confirmation emails
        await _emailService.SendPaymentConfirmationAsync(
            auction.Winner!.Email!,
            auction.Id,
            auction.CurrentPrice);

        await _emailService.SendPaymentReceivedAsync(
            auction.Listing.Seller!.Email!,
            auction.Id,
            auction.CurrentPrice);
    }
}
