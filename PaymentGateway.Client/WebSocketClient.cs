using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class WebSocketClient : IDisposable
{
    private readonly ClientWebSocket _socket = new();
    private readonly string _baseUrl;

    public WebSocketClient(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public async Task ConnectAsync(string token)
    {
        var uri = new Uri(_baseUrl.Replace("https", "wss").Replace("http", "ws") + "/graphql");
        _socket.Options.SetRequestHeader("Authorization", $"Bearer {token}");
        await _socket.ConnectAsync(uri, CancellationToken.None);

        await SendAsync(new
        {
            type = "connection_init",
            payload = new { Authorization = $"Bearer {token}" }
        });

        var response = await ReceiveAsync();
        if (!response.Contains("connection_ack"))
            throw new Exception("WebSocket connection failed: " + response);
    }

    public async Task SubscribeAsync(string query, Action<string> handler)
    {
        await SendAsync(new
        {
            id = "1",
            type = "subscribe",
            payload = new { query }
        });

        _ = Task.Run(async () =>
        {
            while (_socket.State == WebSocketState.Open)
            {
                var message = await ReceiveAsync();
                handler(message);
            }
        });
    }

    private async Task SendAsync(object message)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await _socket.SendAsync(
            new ArraySegment<byte>(bytes),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }

    private async Task<string> ReceiveAsync()
    {
        var buffer = new byte[4096];
        var result = await _socket.ReceiveAsync(
            new ArraySegment<byte>(buffer),
            CancellationToken.None);

        return Encoding.UTF8.GetString(buffer, 0, result.Count);
    }

    public void Dispose()
    {
        _socket.Dispose();
    }
}