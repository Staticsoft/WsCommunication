namespace WsCommunication.Client.Abstractions;

public class WsClientMessage<T>
{
    public required string Path { get; init; }
    public required T Body { get; init; }
}