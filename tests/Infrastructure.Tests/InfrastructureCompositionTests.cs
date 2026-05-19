using RangeExtendedEvDigitalTwin.Infrastructure.Configuration;
using RangeExtendedEvDigitalTwin.Infrastructure.DependencyInjection;

namespace RangeExtendedEvDigitalTwin.Infrastructure.Tests;

public sealed class InfrastructureCompositionTests
{
    [Fact]
    public void Infrastructure_scaffold_types_are_present()
    {
        Assert.Equal("InfrastructureAssemblyMarker", typeof(InfrastructureAssemblyMarker).Name);
        Assert.Equal("simulation", new InfrastructureOptions().EventStoreSchema);
    }
}
