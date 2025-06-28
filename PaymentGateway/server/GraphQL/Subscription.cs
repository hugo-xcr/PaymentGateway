using HotChocolate;
using HotChocolate.Types;
using PaymentGateway.Server.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Server.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic("{paymentId}")]
        public PaymentResult PaymentStatusChanged(
            [ID] string paymentId,
            [EventMessage] PaymentResult payment)
        {
            return payment;
        }
    }
}