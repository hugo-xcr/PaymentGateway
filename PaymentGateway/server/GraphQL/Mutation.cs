using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Server.Data;
using PaymentGateway.Server.Models;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Server.GraphQL;

public class Mutation
{
    public async Task<PaymentResult> InitPayment(
        InitPaymentInput input,
        [Service] AppDbContext context)
    {
        var paymentRequest = new PaymentRequest
        {
            Id = Guid.NewGuid().ToString(),
            Amount = (decimal)input.Amount,
            Currency = input.Currency,
            Method = input.Method,
            Description = input.Description ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        var paymentResult = new PaymentResult
        {
            Id = Guid.NewGuid().ToString(),
            Status = PaymentStatus.INITIATED,
            TransactionId = Guid.NewGuid().ToString("N")[..16],
            PaymentRequest = paymentRequest, // Теперь типы совместимы
            PaymentRequestId = paymentRequest.Id,
            ProcessedAt = DateTime.UtcNow
        };

        context.PaymentRequests.Add(paymentRequest);
        context.PaymentResults.Add(paymentResult);
        await context.SaveChangesAsync();

        return paymentResult;
    }
}

public record InitPaymentInput(
    double Amount,
    string Currency,
    string Method,
    string? Description = null);