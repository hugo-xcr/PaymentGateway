using PaymentGateway.Client.Models;
using PaymentGateway.Server.Models;

public class Subscription
{
    [Subscribe]
    public PaymentResult PaymentStatusChanged(
        [ID] string id,
        [EventMessage] PaymentResult payment)
        => payment;
}