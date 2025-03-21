namespace WsCommunication.Client.Abstractions;

public interface WsConnection : IAsyncDisposable
{
    Task Send<T>(WsClientMessage<T> message);
    Task<string> Receive();
}
