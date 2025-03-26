namespace Staticsoft.WsCommunication.Server.Abstractions;

public class WsServerInMessage<T>
{
    public required string ConnectionId { get; init; }
    public required T Body { get; init; }
}