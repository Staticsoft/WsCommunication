namespace Staticsoft.TestServer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
        => RegisterServices(services);

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        => ConfigureApp(app, env);

    protected virtual IApplicationBuilder ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env) => app
        .UseRouting()
        .UseEndpoints(Endpoints);

    protected virtual IServiceCollection RegisterServices(IServiceCollection services) => services
        .AddControllers().Services;

    void Endpoints(IEndpointRouteBuilder endpoints)
        => ConfigureEndpoints(endpoints);

    protected virtual IEndpointRouteBuilder ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllers();
        return endpoints;
    }
}
