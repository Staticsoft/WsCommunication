using System.IO.Pipelines;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Staticsoft.WsCommunication.Server.Local;

public class LocalWsRequestHandler(
    HttpClient client,
    LocalWsServerConnections connections
)
{
    readonly HttpClient Client = client;
    readonly LocalWsServerConnections Connections = connections;

    public async Task Handle(WebSocket webSocket)
    {
        var connectionId = Connections.Create(webSocket);

        await Client.PostAsJsonAsync("/WebSocket/Connect", new { ConnectionId = connectionId });

        try
        {
            while (true)
            {
                var message = await Receive(webSocket);

                await Client.PostAsync(message.Path, new StringContent(
                    @$"{{""ConnectionId"":""{connectionId}"",""Body"":{message.Body.GetRawText()}}}",
                    Encoding.UTF8,
                    "application/json"
                ));
            }
        }
        catch (WebSocketClosedException ex)
        {
            await webSocket.CloseAsync(ex.Reason, $"{ex.Reason}", CancellationToken.None);
            await Client.PostAsJsonAsync("/WebSocket/Disconnect", new { ConnectionId = connectionId });
        }
    }

    static async Task<Message> Receive(WebSocket webSocket)
    {
        var pipe = new Pipe();
        var message = DeserializeMessage(pipe);
        await ReceiveMessage(pipe, webSocket);
        return await message;
    }

    static async Task ReceiveMessage(Pipe pipe, WebSocket webSocket)
    {
        using var stream = pipe.Writer.AsStream();
        var end = false;
        while (!end)
        {
            var buffer = new byte[4096];
            var received = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (received.CloseStatus.HasValue)
            {
                throw new WebSocketClosedException(received.CloseStatus.Value);
            }

            await stream.WriteAsync(buffer.AsMemory()[..received.Count]);
            end = received.EndOfMessage;
        }
    }

    static async Task<Message> DeserializeMessage(Pipe pipe)
    {
        using var stream = pipe.Reader.AsStream();
        var message = await JsonSerializer.DeserializeAsync<Message>(stream);
        return message!;
    }

    class Message
    {
        public required string Path { get; init; }
        public required JsonElement Body { get; init; }
    }

    class WebSocketClosedException(
        WebSocketCloseStatus status
    ) : Exception($"Connection closed: {status}")
    {
        public WebSocketCloseStatus Reason { get; init; } = status;
    }
}
