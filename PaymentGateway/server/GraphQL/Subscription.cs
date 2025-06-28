using HotChocolate;
using HotChocolate.Types;
using PaymentGateway.Server.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Server.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic("{id}")]
        public PaymentResult PaymentStatusChanged(
            [ID] string id,
            [EventMessage] PaymentResult payment)
        {
            Console.WriteLine($"Отправка обновления для платежа {id}: {payment.Status}");
            return payment;
        }
    }
}