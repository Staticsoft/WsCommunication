using System.Net.WebSockets;

namespace Staticsoft.WsCommunication.Server.Local;

public class LocalWsServerConnections
{
    readonly Dictionary<string, LocalWsServerConnection> Connections = [];

    public LocalWsServerConnection Get(string connectionId)
        => Connections[connectionId];

    public string Create(WebSocket webSocket)
    {
        var id = $"{Guid.NewGuid()}";
        Connections[id] = new(webSocket);
        return id;
    }
}
