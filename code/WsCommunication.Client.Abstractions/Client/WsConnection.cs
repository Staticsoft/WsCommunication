using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Staticsoft.WsCommunication.Client.Abstractions;

public class WsConnection : IAsyncDisposable
{
    readonly WebSocket WebSocket;
    readonly Task ReceiveMessages;
    readonly CancellationTokenSource Cancellation = new();

    public WsConnection(WebSocket webSocket)
    {
        WebSocket = webSocket;
        ReceiveMessages = ReceiveMessagesUntilCancelled(webSocket);
    }

    public Task Send<T>(WsClientOutMessage<T> message)
    {
        var body = $$"""
            {"Path":"{{message.Path}}","Body":{{JsonSerializer.Serialize(message.Body)}}}
            """;
        var bytes = Encoding.UTF8.GetBytes(body);
        return WebSocket.SendAsync(bytes, WebSocketMessageType.Text, true, Cancellation.Token);
    }

    async Task ReceiveMessagesUntilCancelled(WebSocket webSocket)
    {
        while (!Cancellation.IsCancellationRequested)
        {
            try
            {
                var message = await ReceiveMessage(webSocket);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    async Task<string> ReceiveMessage(WebSocket webSocket)
    {
        var bytes = new List<byte>();
        var end = false;
        while (!end)
        {
            var buffer = new byte[4096];
            var received = await webSocket.ReceiveAsync(buffer, Cancellation.Token);
            bytes.AddRange(buffer[..received.Count]);

            end = received.EndOfMessage;
        }
        return Encoding.UTF8.GetString(bytes.ToArray());
    }

    public ChannelReader<T> Receive<T>()
    {

    }

    public async ValueTask DisposeAsync()
    {
        await Cancellation.CancelAsync();
        await ReceiveMessages;
        await WebSocket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            nameof(WebSocketCloseStatus.NormalClosure),
            Cancellation.Token
        );
        WebSocket.Dispose();
    }
}
