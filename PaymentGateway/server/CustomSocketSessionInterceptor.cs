using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.AspNetCore.Subscriptions.Protocols.Apollo;
using HotChocolate.AspNetCore.Subscriptions.Protocols;
using HotChocolate.Execution;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class CustomSocketSessionInterceptor : ISocketSessionInterceptor
{
    public ValueTask<ConnectionStatus> OnConnectAsync(
        ISocketSession session,
        IOperationMessagePayload connectionInitMessage,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"WebSocket connection attempt: {connectionInitMessage}");
        if (connectionInitMessage is InitializeConnectionMessage initMessage &&
            initMessage.Payload is JsonElement json)
        {
            if (json.TryGetProperty("authToken", out var tokenProp) &&
                tokenProp.ValueKind == JsonValueKind.String)
            {
                var token = tokenProp.GetString();
                if (!IsValidToken(token))
                {
                    return ValueTask.FromResult(ConnectionStatus.Reject("Invalid token"));
                }
            }
            else
            {
                return ValueTask.FromResult(ConnectionStatus.Reject("Token required"));
            }
        }
        return ValueTask.FromResult(ConnectionStatus.Accept());
    }

    public ValueTask OnRequestAsync(
        ISocketSession session,
        string operationSessionId,
        OperationRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        var contextData = requestBuilder.GetType().GetProperty("ContextData")?
            .GetValue(requestBuilder) as IDictionary<string, object?>;

        if (contextData != null)
        {
            contextData["sessionId"] = operationSessionId;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask OnCompleteAsync(
        ISocketSession session,
        string operationSessionId,
        CancellationToken cancellationToken) => ValueTask.CompletedTask;

    public ValueTask<IOperationResult> OnResultAsync(
        ISocketSession session,
        string operationSessionId,
        IOperationResult result,
        CancellationToken cancellationToken) => ValueTask.FromResult(result);

    public ValueTask<IReadOnlyDictionary<string, object?>?> OnPingAsync(
        ISocketSession session,
        IOperationMessagePayload pingMessage,
        CancellationToken cancellationToken) =>
        ValueTask.FromResult<IReadOnlyDictionary<string, object?>?>(null);

    public ValueTask OnPongAsync(
        ISocketSession session,
        IOperationMessagePayload pongMessage,
        CancellationToken cancellationToken) => ValueTask.CompletedTask;

    public ValueTask OnCloseAsync(
        ISocketSession session,
        CancellationToken cancellationToken) => ValueTask.CompletedTask;

    private bool IsValidToken(string? token)
    {
        return !string.IsNullOrEmpty(token);
    }
}