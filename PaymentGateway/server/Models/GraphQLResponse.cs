﻿using PaymentGateway.Server.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaymentGateway.Client.Models;

public class GraphQLResponse<T> where T : class
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<GraphQLError>? Errors { get; set; }
}

public class GraphQLError
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    [JsonPropertyName("path")]
    public List<string>? Path { get; set; }

    [JsonPropertyName("locations")]
    public List<GraphQLErrorLocation>? Locations { get; set; }
}

public class GraphQLErrorLocation
{
    [JsonPropertyName("line")]
    public int Line { get; set; }

    [JsonPropertyName("column")]
    public int Column { get; set; }
}

public class PaymentData
{
    [JsonPropertyName("initPayment")]
    public PaymentResult InitPayment { get; set; } = default!;
}