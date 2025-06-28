using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Server.Data;
using PaymentGateway.Server.Models;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Server.GraphQL
{
    public class Mutation
    {
        public async Task<PaymentResult> ConfirmPayment(
    string id,
    [Service] AppDbContext db,
    [Service] ITopicEventSender eventSender)
        {
            var payment = await db.PaymentResults.FindAsync(id);
            payment.Status = PaymentStatus.CONFIRMED;
            await db.SaveChangesAsync();

            await eventSender.SendAsync(id, payment); 
            return payment;
        }
        public async Task<PaymentResult> InitPayment(
            InitPaymentInput input,
            [Service] AppDbContext context)
        {
            if (input.Amount <= 0)
                throw new PaymentException("Amount must be positive");

            var paymentRequest = new PaymentRequest
            {
                Amount = (decimal)input.Amount,
                Currency = input.Currency,
                Method = input.Method,
                Description = input.Description ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            var paymentResult = new PaymentResult
            {
                Status = PaymentStatus.INITIATED,
                TransactionId = "txn_" + Guid.NewGuid().ToString("N")[..16],
                PaymentRequest = paymentRequest,
                ProcessedAt = DateTime.UtcNow
            };

            context.PaymentResults.Add(paymentResult);
            await context.SaveChangesAsync();

            return paymentResult;
        }

        public async Task<PaymentResult> ConfirmPayment(
            [ID] string id,
            [Service] AppDbContext context)
        {
            var payment = await context.PaymentResults
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
                throw new PaymentException("Payment not found");

            payment.Status = PaymentStatus.CONFIRMED;
            await context.SaveChangesAsync();

            return payment;
        }
    }

    public record InitPaymentInput(
        double Amount,
        string Currency,
        string Method,
        string? Description = null);

    public class PaymentException : Exception
    {
        public PaymentException(string message) : base(message) { }
    }

}