namespace PaymentGateway.Server.Models;

public class PaymentRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Method { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}