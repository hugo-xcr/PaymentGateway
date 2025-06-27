// PaymentResultDto.cs в клиентском проекте
using PaymentGateway.Server.Models;
using System;
using System.Text.Json.Serialization;

namespace PaymentGateway.Server.server.Models;

public class PaymentResultDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatus Status { get; set; }

    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("processedAt")]
    public DateTime ProcessedAt { get; set; }

    [JsonPropertyName("paymentRequest")]
    public PaymentRequestDto PaymentRequest { get; set; }
}

public class PaymentRequestDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}