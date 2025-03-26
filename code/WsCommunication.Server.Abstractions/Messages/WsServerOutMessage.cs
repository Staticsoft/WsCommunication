namespace Staticsoft.WsCommunication.Server.Abstractions;

public class WsServerOutMessage<T>
{
    public required string Type { get; init; }
    public required T Body { get; init; }
}