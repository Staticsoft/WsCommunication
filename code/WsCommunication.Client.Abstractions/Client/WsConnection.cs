using System.Threading.Channels;

namespace Staticsoft.WsCommunication.Client.Abstractions;

public interface WsConnection : IAsyncDisposable
{
    Task Send<T>(WsClientOutMessage<T> message);
    ChannelReader<T> Receive<T>();
}
