using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Server.server.Models
{
    public class PaymentGatewaySettings
    {
        public string DefaultCurrency { get; set; } = "USD";
        public List<string> AllowedPaymentMethods { get; set; } = new();
    }
}
