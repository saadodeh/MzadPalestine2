using System;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.DTOs.Wallet;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}