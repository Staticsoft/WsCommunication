namespace Staticsoft.TestServer.AWS;

public class AWSStartup : Startup
{
    protected override IServiceCollection RegisterServices(IServiceCollection services)
        => base.RegisterServices(services);

    static string Configuration(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new NullReferenceException($"Environment varialbe {name} is not set");

    protected override IEndpointRouteBuilder ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        => base.ConfigureEndpoints(endpoints);
}