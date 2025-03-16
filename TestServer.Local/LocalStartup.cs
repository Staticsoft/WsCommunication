namespace Staticsoft.TestServer.Local;

public class LocalStartup : Startup
{
    protected override IServiceCollection RegisterServices(IServiceCollection services)
        => base.RegisterServices(services);

    protected override IApplicationBuilder ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
    {
        base.ConfigureApp(app, env);

        return app;
    }

    protected override IEndpointRouteBuilder ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        base.ConfigureEndpoints(endpoints);

        return endpoints;
    }
}