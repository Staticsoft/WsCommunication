namespace Staticsoft.WsCommunication.Server.Abstractions;

public interface WsServer
{
    Task Send(string connectionId, byte[] data);
}