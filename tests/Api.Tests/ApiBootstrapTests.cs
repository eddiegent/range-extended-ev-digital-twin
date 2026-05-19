using RangeExtendedEvDigitalTwin.Api.Realtime;

namespace RangeExtendedEvDigitalTwin.Api.Tests;

public sealed class ApiBootstrapTests
{
    [Fact]
    public void Simulation_hub_shell_is_present()
    {
        Assert.Equal("SimulationHub", typeof(SimulationHub).Name);
    }
}
