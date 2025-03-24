namespace Staticsoft.TestServer;

public interface WsManagement
{
    Task Send(string connectionId, byte[] data);
}