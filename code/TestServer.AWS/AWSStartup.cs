using Amazon.ApiGatewayManagementApi;
using Staticsoft.WsCommunication.Server.ApiGateway;

namespace Staticsoft.TestServer.AWS;

public class AWSStartup : Startup
{
    protected override IServiceCollection RegisterServices(IServiceCollection services)
        => base.RegisterServices(services)
            .UseApiGatewayWsServer(
                p => new AmazonApiGatewayManagementApiClient(
                    p.GetRequiredService<AmazonApiGatewayManagementApiConfig>()
                ),
                _ => new() { ServiceURL = Configuration("ApiGatewayEndpoint") }
            );

    static string Configuration(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new NullReferenceException($"Environment varialbe {name} is not set");
}
