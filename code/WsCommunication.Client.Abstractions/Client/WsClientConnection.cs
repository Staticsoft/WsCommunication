using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Staticsoft.WsCommunication.Client.Abstractions;

public class WsClientConnection : WsConnection
{
    readonly WebSocket WebSocket;
    readonly Task ReceiveMessages;
    readonly CancellationTokenSource Cancellation = new();
    readonly ConcurrentDictionary<string, Channel<JsonElement>> Channels = [];

    public WsClientConnection(WebSocket webSocket)
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
                var channel = GetChannel(message.Type);
                await channel.Writer.WriteAsync(message.Body);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    async Task<Message> ReceiveMessage(WebSocket webSocket)
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
        return JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(bytes.ToArray()))!;
    }

    public ChannelReader<T> Receive<T>()
    {
        var jsonChannel = GetChannel(typeof(T).Name);
        var channel = Channel.CreateUnbounded<T>();
        _ = Task.Run(async () =>
        {
            while (await jsonChannel.Reader.WaitToReadAsync(Cancellation.Token))
            {
                var message = await jsonChannel.Reader.ReadAsync(Cancellation.Token);
                await channel.Writer.WriteAsync(JsonSerializer.Deserialize<T>(message)!);
            }
        }, Cancellation.Token);
        return channel.Reader;
    }

    Channel<JsonElement> GetChannel(string name)
        => Channels.GetOrAdd(name, (_) => Channel.CreateUnbounded<JsonElement>());

    public async ValueTask DisposeAsync()
    {
        await Cancellation.CancelAsync();
        await ReceiveMessages;

        if (WebSocket.State == WebSocketState.Open)
        {
            await WebSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                nameof(WebSocketCloseStatus.NormalClosure),
                CancellationToken.None
            );
        }

        WebSocket.Dispose();
    }

    class Message
    {
        public required string Type { get; init; }
        public required JsonElement Body { get; init; }
    }
}
