using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentGateway.Client.Models;

public class PaymentClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public PaymentClient(string baseUrl, string jwtToken)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    public async Task<PaymentResultDto> InitPaymentAsync(InitPaymentInput input)
    {
        var request = new
        {
            query = """
    mutation InitPayment($input: InitPaymentInput!) {
      initPayment(input: $input) {
        id
        status
        transactionId
        paymentRequest {
          id
          amount
          currency
          method
          description
          createdAt
        }
      }
    }
    """,
            variables = new { input }
        };

        var response = await _httpClient.PostAsJsonAsync("graphql", request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"GraphQL request failed: {content}");
        }

        var result = JsonSerializer.Deserialize<GraphQLResponse<PaymentDataDto>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Data?.InitPayment ?? throw new InvalidOperationException("Invalid response");
    }

    internal async Task InitPaymentAsync(global::InitPaymentInput initPaymentInput)
    {
        throw new NotImplementedException();
    }

    public class PaymentDataDto
    {
        public PaymentResultDto InitPayment { get; set; }
    }
}