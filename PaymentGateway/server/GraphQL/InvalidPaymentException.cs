namespace PaymentGateway.Server.GraphQL;

public class InvalidPaymentException : Exception
{
    public InvalidPaymentException(string message) : base(message) { }
}