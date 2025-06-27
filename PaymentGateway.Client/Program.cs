using PaymentGateway.Client.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static PaymentGateway.Client.Models.PaymentClient;

namespace PaymentGateway.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            const string baseUrl = "http://localhost:5000";
            const string jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0X3VzZXIiLCJqdGkiOiI4ODFjZDE1OS01MGE1LTQ5YjEtOWFjMi00MmIyODM5ODg4ZjkiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNzUxMDM5NDA1LCJpc3MiOiJQYXltZW50R2F0ZXdheVNlcnZlciIsImF1ZCI6IlBheW1lbnRHYXRld2F5Q2xpZW50In0.F2mr2KaXcZ8zT8KaHRiB6HCb403A_KmhPIjOGaxQ3VU";

            Console.WriteLine("Проверка подключения к серверу...");
            try
            {
                using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);

                var response = await httpClient.GetAsync("/health");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Статус подключения: {response.StatusCode}");
                Console.WriteLine($"Ответ сервера: {content}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка подключения: {content}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                return;
            }

            Console.WriteLine("Инициализация платежа...");
            try
            {
                var client = new PaymentClient(baseUrl, jwtToken);
                var payment = await client.InitPaymentAsync(new InitPaymentInput(
                    Amount: 100.0,
                    Currency: "USD",
                    Method: "CreditCard",
                    Description: "Test payment"
                ));

                Console.WriteLine("Платеж успешно создан:");
                Console.WriteLine($"ID: {payment.Id}");
                Console.WriteLine($"Статус: {payment.Status}");
                Console.WriteLine($"Transaction ID: {payment.TransactionId}");

                if (payment.PaymentRequest != null)
                {
                    Console.WriteLine($"Сумма: {payment.PaymentRequest.Amount} {payment.PaymentRequest.Currency}");
                    Console.WriteLine($"Метод: {payment.PaymentRequest.Method}");
                    Console.WriteLine($"Описание: {payment.PaymentRequest.Description ?? "N/A"}");
                    Console.WriteLine($"Дата создания: {payment.PaymentRequest.CreatedAt}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка платежа: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }

    public class PaymentClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PaymentClient(string baseUrl, string jwtToken)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);
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

            var result = JsonSerializer.Deserialize<GraphQLResponse<PaymentDataDto>>(content, _jsonOptions);
            return result?.Data?.InitPayment ?? throw new InvalidOperationException("Invalid response");
        }
    }
}