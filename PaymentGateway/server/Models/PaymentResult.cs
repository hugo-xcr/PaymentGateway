// PaymentGateway.Server/Models/PaymentResult.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Server.Models;

[Table("PaymentResults")]
public class PaymentResult
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public PaymentStatus Status { get; set; }

    public string? TransactionId { get; set; }

    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("PaymentRequest")]
    public string PaymentRequestId { get; set; }

    // Изменяем тип на серверную модель
    public PaymentRequest PaymentRequest { get; set; }
}