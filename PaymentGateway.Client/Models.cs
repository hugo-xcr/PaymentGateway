using PaymentGateway.Server.Models;

namespace PaymentGateway.Client.Models;

public class GraphQLResponse<T> where T : class
{
    public T Data { get; set; } = default!;
    public List<GraphQLError>? Errors { get; set; }
}

public class GraphQLError
{
    public string Message { get; set; } = default!;
}

public class PaymentData
{
    public PaymentResult InitPayment { get; set; } = default!;
}

public record InitPaymentInput(
    double Amount,
    string Currency,
    string Method,
    string? Description = null);