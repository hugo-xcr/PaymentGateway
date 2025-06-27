using System;
using System.Text.Json.Serialization;

namespace PaymentGateway.Client.Models;

public class PaymentResultDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatus Status { get; set; }

    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("paymentRequest")]
    public PaymentRequestDto PaymentRequest { get; set; } = null!;
}