namespace WsCommunication.Client.Abstractions;

public interface WsClient
{
    Task<WsConnection> Connect();
}
