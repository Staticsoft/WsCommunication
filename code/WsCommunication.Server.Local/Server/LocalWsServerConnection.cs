using System.Net.WebSockets;
using System.Text;

namespace Staticsoft.WsCommunication.Server.Local;

public class LocalWsServerConnection(
    WebSocket webSocket
)
{
    readonly WebSocket WebSocket = webSocket;

    public Task Send(string message)
        => WebSocket.SendAsync(
            Encoding.UTF8.GetBytes(message),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None
        );
}
