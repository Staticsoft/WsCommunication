namespace Staticsoft.TestContracts;

public class Schema(
    WebSocket webSocket
)
{
    public WebSocket WebSocket { get; } = webSocket;
}
