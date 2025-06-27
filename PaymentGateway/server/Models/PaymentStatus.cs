using System.Text.Json.Serialization;

namespace PaymentGateway.Server.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatus
{
    INITIATED = 0,
    CONFIRMED = 1,
    FAILED = 2,
    REFUNDED = 3
}