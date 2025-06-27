public record InitPaymentInput(
    double Amount,
    string Currency,
    string Method,
    string? Description = null);