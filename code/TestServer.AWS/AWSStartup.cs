using Amazon.ApiGatewayManagementApi;

namespace Staticsoft.TestServer.AWS;

public class AWSStartup : Startup
{
    protected override IServiceCollection RegisterServices(IServiceCollection services)
        => base.RegisterServices(services)
            .AddSingleton<WsManagement, ApiGatewayWsManagement>()
            .AddSingleton<AmazonApiGatewayManagementApiClient>()
            .AddSingleton(ApiGatewayConfig());

    static AmazonApiGatewayManagementApiConfig ApiGatewayConfig()
        => new() { ServiceURL = Configuration("ApiGatewayEndpoint") };

    static string Configuration(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new NullReferenceException($"Environment varialbe {name} is not set");
}
