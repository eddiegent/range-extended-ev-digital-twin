using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;
using RangeExtendedEvDigitalTwin.Infrastructure.Authentication;
using RangeExtendedEvDigitalTwin.Infrastructure.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.Persistence;

namespace RangeExtendedEvDigitalTwin.Infrastructure.Tests;

public sealed class InfrastructureCompositionTests
{
    [Fact]
    public void Infrastructure_registers_identity_and_postgres_backed_services()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>(
                    "ConnectionStrings:simulationdb",
                    "Host=localhost;Port=5432;Database=simulationdb;Username=postgres;Password=postgres")
            ])
            .Build();

        var services = new ServiceCollection();

        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();

        Assert.Equal("InfrastructureAssemblyMarker", typeof(InfrastructureAssemblyMarker).Name);
        Assert.Equal("auth", new InfrastructureOptions().AuthenticationSchema);
        Assert.NotNull(provider.GetService<SimulationDbContext>());
        Assert.NotNull(provider.GetService<IUserStore<OperatorUser>>());
    }
}
