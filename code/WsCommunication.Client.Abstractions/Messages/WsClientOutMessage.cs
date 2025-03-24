namespace Staticsoft.WsCommunication.Client.Abstractions;

public class WsClientOutMessage<T>
{
    public required string Path { get; init; }
    public required T Body { get; init; }
}