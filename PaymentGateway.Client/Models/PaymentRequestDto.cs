namespace PaymentGateway.Client.Models;

public class PaymentRequestDto
{
    public string Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Method { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}