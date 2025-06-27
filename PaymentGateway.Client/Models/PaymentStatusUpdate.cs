using PaymentGateway.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Client.Models
{
    public record PaymentStatusUpdate
    {
        public PaymentResult PaymentStatusChanged { get; set; }
    }
}
