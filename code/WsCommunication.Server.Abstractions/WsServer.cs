namespace Staticsoft.WsCommunication.Server.Abstractions;

public interface WsServer
{
    Task Send<T>(string connectionId, T message);
}