using RangeExtendedEvDigitalTwin.Api.Realtime;
using RangeExtendedEvDigitalTwin.Application.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet(
    "/",
    () => Results.Ok(
        new
        {
            service = "RangeExtended EV Digital Twin API",
            stage = "scaffolding",
            capabilities = new[]
            {
                "health",
                "signalr-shell",
                "identity-store",
                "service-discovery",
                "infrastructure-wiring"
            }
        }));

app.MapHub<SimulationHub>("/hubs/simulation");

app.Run();

public partial class Program;
