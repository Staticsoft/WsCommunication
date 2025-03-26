using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.WsCommunication.Server.Local;

class LocalWsServer(
    LocalWsServerConnections connections
) : WsServer
{
    readonly LocalWsServerConnections Connections = connections;

    public Task Send<T>(string connectionId, T message)
        => Connections.Get(connectionId).Send(message);
}