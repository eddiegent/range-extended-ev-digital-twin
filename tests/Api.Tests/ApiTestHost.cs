using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RangeExtendedEvDigitalTwin.Api.Authentication;
using RangeExtendedEvDigitalTwin.Application.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.DependencyInjection;

namespace RangeExtendedEvDigitalTwin.Api.Tests;

public sealed class ApiTestHost : IAsyncDisposable
{
    private readonly WebApplication _app;

    private ApiTestHost(WebApplication app)
    {
        _app = app;
        Client = app.GetTestClient();
    }

    public HttpClient Client { get; }

    public IServiceProvider Services => _app.Services;

    public static async Task<ApiTestHost> StartAsync()
    {
        var builder = WebApplication.CreateBuilder();
        var databaseName = $"api-tests-{Guid.NewGuid()}";

        builder.WebHost.UseTestServer();
        builder.Configuration.AddInMemoryCollection(
        [
            new KeyValuePair<string, string?>("BootstrapOperator:Enabled", "true"),
            new KeyValuePair<string, string?>("BootstrapOperator:UserName", "demo.operator"),
            new KeyValuePair<string, string?>("BootstrapOperator:Email", "operator@local.test"),
            new KeyValuePair<string, string?>("BootstrapOperator:Password", "Passw0rd")
        ]);

        builder.Services.AddProblemDetails();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(
            builder.Configuration,
            options => options.UseInMemoryDatabase(databaseName));
        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapAuthEndpoints();

        await app.StartAsync();

        return new ApiTestHost(app);
    }

    public async ValueTask DisposeAsync()
    {
        await _app.StopAsync();
        await _app.DisposeAsync();
    }
}
